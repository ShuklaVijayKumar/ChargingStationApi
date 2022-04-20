using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository
{
    public interface ICosmosFeedReader
    { /// <summary>
      /// Gets items from cosmos using the provided query.
      /// </summary>
      /// <typeparam name="T">The type of data.</typeparam>
      /// <param name="query">The query.</param>
      /// <param name="toFeedIterator">Method which coverts a query into a feed iterator.</param>
      /// <returns>Items matching the query.</returns>
      Task<List<T>> GetItems<T>(IQueryable<T> query, Func<IQueryable<T>, FeedIterator<T>> toFeedIterator);
    }
}
