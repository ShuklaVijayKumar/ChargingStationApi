using System;
using System.Linq.Expressions;

namespace ChargingStationApi.Repository
{
    /// <summary>
    /// Class used to wrap the configuration of the DB order by clause for a specific property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The attribute value type.</typeparam>
    public class OrderByWrapper<TEntity, TKey>
    {
        /// <summary>
        /// Gets or sets the expression used to retrieve the attribute value.
        /// </summary>
        public Expression<Func<TEntity, TKey>> Attribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute should be sorted in ascending or descending.
        /// </summary>
        public bool Ascending { get; set; } = true;
    }
}
