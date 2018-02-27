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
    public class InnerTransactionsRepository : BaseRepository<InnerTransaction,int>, IInnerTransactionsRepository
    {
        public InnerTransactionsRepository(CoinDriveContext context) : base(context)
        {
        }

        protected override Expression<Func<InnerTransaction, bool>> KeyPredicate(int entityId) =>
            (x => x.Id == entityId);

        protected override IQueryable<InnerTransaction> NavigationPropertiesIncluder(
            IQueryable<InnerTransaction> sequence) => sequence.Include(x => x.User).Include(x => x.AssociatedTransaction);
    }
}