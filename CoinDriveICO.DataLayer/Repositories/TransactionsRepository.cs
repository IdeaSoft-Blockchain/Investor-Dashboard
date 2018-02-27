using System;
using System.Linq;
using System.Linq.Expressions;
using CoinDriveICO.DataLayer.Context;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories.Base;
using CoinDriveICO.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoinDriveICO.DataLayer.Repositories
{
    public class TransactionsRepository : BaseRepository<Transaction,string>, ITransactionsRepository
    {
        public TransactionsRepository(CoinDriveContext context) : base(context)
        {
        }

        protected override Expression<Func<Transaction, bool>> KeyPredicate(string entityId) =>
            (transaction => transaction.TxKey == entityId);

        protected override IQueryable<Transaction> NavigationPropertiesIncluder(IQueryable<Transaction> sequence) =>
            sequence.Include(transaction => transaction.User);
    }
}