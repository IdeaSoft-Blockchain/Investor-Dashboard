using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Model;

namespace CoinDriveICO.BusinessLayer.Services
{
    public interface ITransactionsService
    {
        Task<Transaction> GetByIdAsync(string hash);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction> InsertTransaction();
    }
    public class TransactionsService : ITransactionsService
    {
        private readonly IAdapterApiService _adapterApiService;

        public TransactionsService(IAdapterApiService adapterApiService)
        {
            _adapterApiService = adapterApiService;
        }

        public Task<Transaction> GetByIdAsync(string hash)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Transaction> InsertTransaction()
        {
            throw new System.NotImplementedException();
        }

    }
}