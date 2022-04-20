using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository.Interfaces
{
    /// <summary>
    /// IRespository interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets all entities, filtered and sorted using the provided criteria.
        /// </summary>
        /// <param name="skip">Optional number of items to skip.</param>
        /// <param name="top">Optional maximum number of items. This is always capped at the container resource item limit (regardless of the provided value), defined in the app configuration.</param>
        /// <returns>The resulting collection of entities.</returns>
        ValueTask<ObjectResult> GetAllAsync(
            Expression<Func<TEntity, bool>> existsPredicate,
            int? skip = null,
            int? top = null);

        /// <summary>
        /// Gets a single entity.
        /// </summary>
        /// <param name="id">The cosmos document identifier.</param>
        /// <returns>A single entity.</returns>
        ValueTask<ObjectResult> GetAsync(string id);

        /// <summary>
        /// Deletes a single entity.
        /// </summary>
        /// <param name="id">The cosmos document identifier.</param>
        /// <returns>Delete entity response.</returns>
        ValueTask<ObjectResult> DeleteAsync(string id);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>The newly created entity.</returns>
        ValueTask<ObjectResult> AddAsync(TEntity entity, string partitionKey);
    }
}
