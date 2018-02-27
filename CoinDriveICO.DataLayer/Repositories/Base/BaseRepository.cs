using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Context;
using CoinDriveICO.DataLayer.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace CoinDriveICO.DataLayer.Repositories.Base
{

    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity
    {
        /// <summary>
        /// Name of user which should be written to entity when it is being updated or created
        /// </summary>
        private const string CreateUpdateUser = "Server";

        /// <summary>
        /// DB context
        /// </summary>
        private readonly CoinDriveContext _context;

        /// <summary>
        /// <see cref="TEntity"/> DB set tracked by context
        /// </summary>
        private readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(CoinDriveContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// DB set non tracked by context
        /// </summary>
        private IQueryable<TEntity> NoTrackingSet => _dbSet.AsNoTracking();

        /// <summary>
        /// Expression that should compile into <paramref name="entityId"/> to primary key equality function
        /// </summary>
        /// <param name="entityId">Entity key to compare</param>
        /// <returns></returns>
        protected abstract Expression<Func<TEntity, bool>> KeyPredicate(TKey entityId);

        /// <summary>
        /// Method that maps navigation properties to specified sequence
        /// </summary>
        /// <param name="sequence">Sequence to be mapped</param>
        /// <returns></returns>
        protected abstract IQueryable<TEntity> NavigationPropertiesIncluder(IQueryable<TEntity> sequence);

        /// <summary>
        /// Sets create date of <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Entity which create date is being set</param>
        /// <returns>Entity with new create date</returns>
        private static TEntity SetEntityCreateDate(TEntity entity)
        {
            entity.CreateUser = entity.UpdateUser = CreateUpdateUser;
            entity.CreateDate = entity.UpdateDate = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// Sets update date of <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Entity which update date is being set</param>
        /// <returns>Entity with new update date</returns>
        private static TEntity SetEntityUpdateDate(TEntity entity)
        {
            entity.UpdateUser = CreateUpdateUser;
            entity.UpdateDate = DateTime.Now;
            return entity;
        }

        #region Asynchronous method versions

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetSingleByKeyAsync(TKey key, bool includeNavigationProps = true)
        {
            var resultSet = NoTrackingSet.Where(KeyPredicate(key));
            if (includeNavigationProps)
                resultSet = NavigationPropertiesIncluder(resultSet);
            return await resultSet.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool includeNavigationProps = true)
        {
            return await GetAllAsync(x => x, includeNavigationProps);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> linqChain, bool includeNavigationProps = true)
        {
            var linqChained = linqChain(NoTrackingSet);
            if (includeNavigationProps)
                linqChained = NavigationPropertiesIncluder(linqChained);
            return await linqChained.ToArrayAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> ContainsAsync(TEntity entity)
        {
            return await _dbSet.ContainsAsync(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<bool> ContainsAsync(TKey id)
        {
            var entity = await NoTrackingSet.FirstOrDefaultAsync(KeyPredicate(id));
            return await _dbSet.ContainsAsync(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity = SetEntityCreateDate(entity);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = SetEntityUpdateDate(entity);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> DeleteAsync(TKey key)
        {
            var entity = await GetSingleByKeyAsync(key);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion
        
        public IEnumerator<TEntity> GetEnumerator() => NoTrackingSet.GetEnumerator();
        public Type ElementType => NoTrackingSet.ElementType;
        public Expression Expression => NoTrackingSet.Expression;
        public IQueryProvider Provider => NoTrackingSet.Provider;
        IEnumerator IEnumerable.GetEnumerator() => NoTrackingSet.GetEnumerator();
    }
}