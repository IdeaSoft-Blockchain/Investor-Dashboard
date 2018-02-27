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
    public class AffiliatePayoffsRepository : BaseRepository<AffiliatePayoff,int>, IAffiliatePayoffsRepository
    {
        public AffiliatePayoffsRepository(CoinDriveContext context) : base(context)
        {
        }

        protected override Expression<Func<AffiliatePayoff, bool>> KeyPredicate(int entityId) =>
            (x => x.Id == entityId);

        protected override IQueryable<AffiliatePayoff> NavigationPropertiesIncluder(
            IQueryable<AffiliatePayoff> sequence) =>
            (sequence.Include(x => x.AffiliateUser).Include(x => x.PayingUser).Include(x => x.InnerTransaction));
    }
}