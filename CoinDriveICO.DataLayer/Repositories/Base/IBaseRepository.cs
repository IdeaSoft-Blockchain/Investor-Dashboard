using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Model.Base;

namespace CoinDriveICO.DataLayer.Repositories.Base
{
    /// <summary>
    /// Contract for repository implementations, that implements basic CRUD functionality
    /// </summary>
    /// <typeparam name="TEntity">Type of entity class, that should implement <see cref="IBaseEntity"/> interface</typeparam>
    /// <typeparam name="TKey">Type of records primary key</typeparam>
    public interface IBaseRepository<TEntity, in TKey> : IQueryable<TEntity> where TEntity : class, IBaseEntity
    {
        /// <summary>
        /// Asynchronously gets single untracked entity from DB set described by <paramref name="key"/>. 
        /// When <paramref name="includeNavigationProps"/> is set to true returned entity will 
        /// include navigation properties. 
        /// </summary>
        /// <param name="key">Primary key of record in DB</param>
        /// <param name="includeNavigationProps">Should entity navigation properties be mapped</param>
        /// <returns>Single untracked entity</returns>
        Task<TEntity> GetSingleByKeyAsync(TKey key, bool includeNavigationProps = true);

        /// <summary>
        /// Asynchronously gets whole untracked DB set
        /// </summary>
        /// <param name="includeNavigationProps">Should entities navigation properties be mapped</param>
        /// <returns>Whole untracked DB set</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(bool includeNavigationProps = true);

        /// <summary>
        /// Asynchronously gets whole untracked DB set, affected by LINQ query specified by <paramref name="linqChain"/>
        /// </summary>
        /// <param name="linqChain">LINQ query which affects basic sequence</param>
        /// <param name="includeNavigationProps">Should entities navigation properties be mapped</param>
        /// <returns>Whole untracked DB set</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> linqChain,
            bool includeNavigationProps = true);

        /// <summary>
        /// Asynchronously checks an existance of provided <paramref name="entity"/> in DB set
        /// </summary>
        /// <param name="entity">Entity which existance is being checked</param>
        /// <returns>Boolean value that represents existance of element in the DB set</returns>
        Task<bool> ContainsAsync(TEntity entity);

        /// <summary>
        /// Asynchronously checks an existance of element with provided key in DB set
        /// </summary>
        /// <param name="id">Primary key of record in DB</param>
        /// <returns>Boolean value that represents existance of element in the DB set</returns>
        Task<bool> ContainsAsync(TKey id);

        /// <summary>
        /// Asynchronously inserts entity to repository DB set
        /// </summary>
        /// <param name="entity">Inserted entity, which shouldn't contain key</param>
        /// <returns>Inserted entity with new ID</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Asynchronously updates entity in repository DB set
        /// </summary>
        /// <param name="entity">Entity which will be updated, should contain primary key</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Asynchronously deletes <paramref name="entity"/> from DB set
        /// </summary>
        /// <param name="entity">Entity which should have set primary key</param>
        /// <returns>Deleted entity</returns>
        Task<TEntity> DeleteAsync(TEntity entity);

        /// <summary>
        /// Asynchronously delets entity specified by <paramref name="key"/> from DB set
        /// </summary>
        /// <param name="key">Primary key of record</param>
        /// <returns>Deleted entity</returns>
        Task<TEntity> DeleteAsync(TKey key);
    }
}