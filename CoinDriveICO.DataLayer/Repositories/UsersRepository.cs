using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Context;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories.Base;
using CoinDriveICO.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoinDriveICO.DataLayer.Repositories
{
    public class UsersRepository : BaseRepository<AppUser,int>, IUsersRepository
    {
        public UsersRepository(CoinDriveContext context) : base(context)
        {
        }

        protected override Expression<Func<AppUser, bool>> KeyPredicate(int entityId) => (user => user.Id == entityId);

        protected override IQueryable<AppUser> NavigationPropertiesIncluder(IQueryable<AppUser> sequence) =>
            sequence.Include(x => x.AffiliateUser);
    }
}