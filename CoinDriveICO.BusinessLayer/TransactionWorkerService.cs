using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinDriveICO.BusinessLayer.Services;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories.Interfaces;
using CoinDriveICO.Framework.JsonStructures.AdapterApi;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.BusinessLayer
{
    public class TransactionWorkerService : ITransactionWorkerService
    {
        private readonly IUsersService _usersService;
        private readonly IAdapterApiService _adapterApiService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IInnerTransactionsRepository _innerTransactionsRepository;
        private readonly IAffiliatePayoffsRepository _affiliatePayoffsRepository;

        private readonly decimal _ethToTokenRate;
        private readonly decimal _btcToTokenRate;
        private readonly decimal _tokenMultiplierRate;
        private readonly decimal _affiliationPaybackMultiplierRate;
        public TransactionWorkerService(IUsersService usersService, 
            IAdapterApiService adapterApiService, 
            ITransactionsRepository transactionsRepository,
            IInnerTransactionsRepository innerTransactionsRepository,
            IAffiliatePayoffsRepository affiliatePayoffsRepository,
            IOptions<MainSettings> options)
        {
            _usersService = usersService;
            _adapterApiService = adapterApiService;
            _ethToTokenRate = options.Value.EthToTokenRate;
            _btcToTokenRate = options.Value.BtcToTokenRate;
            _tokenMultiplierRate = options.Value.TokenMultiplier;
            _affiliationPaybackMultiplierRate = options.Value.AffiliatorPaybackMultiplierRate;
            _transactionsRepository = transactionsRepository;
            _innerTransactionsRepository = innerTransactionsRepository;
            _affiliatePayoffsRepository = affiliatePayoffsRepository;

        }

        public async Task<IEnumerable<Transaction>> ProcessTransactionsAsync()
        {
            var transactionsToProcess = await DumpTransactionsToDbAsync();
            var processingFailedTransactions = new List<Transaction>();
            foreach (var transaction in transactionsToProcess)
            {
                var processingResult = await ProcessSingleTransactionAsync(transaction);
                if (processingResult is null)
                {
                    processingFailedTransactions.Add(transaction);
                }
                else
                {
                    await MarkTransactionAsProcessedAsync(transaction);
                }
            }

            IEnumerable<string> GetProcessedIdsByAdapterName(string adapterName) =>
                transactionsToProcess.Where(x => processingFailedTransactions.All(y => y.TxKey != x.TxKey) && x.Symbol == adapterName)
                    .Select(x => x.TxKey);

            var ethProcessedIds = GetProcessedIdsByAdapterName("ETH");
            await _adapterApiService.MarkTransactionAsConfirmed(ethProcessedIds, "ETH");

            var btcProcessedIds = GetProcessedIdsByAdapterName("BTC");
            await _adapterApiService.MarkTransactionAsConfirmed(btcProcessedIds, "BTC");

            return processingFailedTransactions;
        }


        private async Task<IEnumerable<Transaction>> DumpTransactionsToDbAsync()
        {
            var btcTransactions = (await _adapterApiService.GetTransactionHistoryAsync("BTC")).Value;
            var ethTransactions = (await _adapterApiService.GetTransactionHistoryAsync("ETH")).Value;

            //TODO: Fix multiple enumeration
            var convertedTransactionsList = new List<Transaction>(btcTransactions.Count() + ethTransactions.Count());

            foreach (var transaction in ethTransactions.Concat(btcTransactions))
            {
                var convertedTransaction = await ConvertResponseToModelAndInsertAsync(transaction);
                convertedTransactionsList.Add(convertedTransaction);
            }

            return convertedTransactionsList;
        }

        private async Task<Transaction> ConvertResponseToModelAndInsertAsync(TransactionInfo responseTransaction)
        {
            var result = new Transaction
            {
                Address = responseTransaction.Address,
                Amount = responseTransaction.Amount,
                Confirmations = responseTransaction.Confirmations,
                IsProcessed = false,
                From = responseTransaction.From,
                Memo = responseTransaction.Memo,
                Symbol = responseTransaction.Symbol,
                Time = responseTransaction.Time,
                TxKey = responseTransaction.TxKey,
                TxHash = responseTransaction.TxHash,
                UserId = responseTransaction.UserId
            };
            result = await _transactionsRepository.InsertAsync(result);
            return result;
        }

        private async Task<InnerTransaction> ProcessSingleTransactionAsync(Transaction transaction)
        {
            if (!transaction.IsProcessed)
            {
                var innerTransaction = CreateInnerTransaction(transaction);
                innerTransaction = await ProcessAndInsertInnerTransactionAsync(innerTransaction);
                await PayToAffiliatorAsync(innerTransaction.UserId, innerTransaction.ToValue, innerTransaction);
                return innerTransaction;
            }
            return null;
        }

        private async Task<Transaction> MarkTransactionAsProcessedAsync(Transaction transaction)
        {
            transaction.IsProcessed = true;
            transaction = await _transactionsRepository.UpdateAsync(transaction);
            return transaction;
        }

        private InnerTransaction CreateInnerTransaction(Transaction source)
        {
            var result = new InnerTransaction
            {
                UserId = source.UserId,
                AssociatedTransactionId = source.TxKey,
                FromType = source.Symbol,
                FromValue = source.Amount,
                ToValue = source.Amount,
                TokenMultiplier = _tokenMultiplierRate,
            };
            return result;
        }

        private async Task<InnerTransaction> ProcessAndInsertInnerTransactionAsync(InnerTransaction transaction)
        {
            transaction.ToValue *= _tokenMultiplierRate;
            switch (transaction.FromType)
            {
                case ("BTC"):
                    transaction.TypeToTokenConversationRate = _btcToTokenRate;
                    transaction.ToValue *= _btcToTokenRate;
                    break;
                case ("ETH"):
                    transaction.TypeToTokenConversationRate = _ethToTokenRate;
                    transaction.ToValue *= _ethToTokenRate;
                    break;
                default:
                    throw new ArgumentException("Invalid transaction adapter: " + transaction.FromType);
            }
            await _usersService.IncreaseUserBalance(transaction.UserId, transaction.ToValue);
            transaction = await _innerTransactionsRepository.InsertAsync(transaction);
            return transaction;
        }
    
        private async Task<bool> PayToAffiliatorAsync(int userId, decimal userPaymentValue, InnerTransaction transaction)
        {
            var affiliatorUser = await _usersService.GetAffiliatorOfUser(userId);
            if (affiliatorUser == null) return false;
            var affiliatorPaybackValue = userPaymentValue * _affiliationPaybackMultiplierRate;
            await _usersService.IncreaseUserBalance(affiliatorUser.Id, affiliatorPaybackValue);
            await _affiliatePayoffsRepository.InsertAsync(new AffiliatePayoff
            {
                AffiliatePayoffMultiplier = _affiliationPaybackMultiplierRate,
                AffiliateUserId = affiliatorUser.Id,
                InnerTransactionId = transaction.Id,
                PayingUserId = userId,
                TransactionValue = transaction.ToValue,
                PayoffValue = affiliatorPaybackValue
            });
            return true;
        }
    }
}