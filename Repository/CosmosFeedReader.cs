using Ardalis.GuardClauses;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository
{
    public class CosmosFeedReader : ICosmosFeedReader
    {
        public async Task<List<T>> GetItems<T>(IQueryable<T> query, Func<IQueryable<T>, FeedIterator<T>> toFeedIterator)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.Null(toFeedIterator, nameof(toFeedIterator));

            List<T> items = new List<T>();
            using (var feedIterator = toFeedIterator(query))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await feedIterator.ReadNextAsync().ConfigureAwait(false);
                    foreach (T item in response)
                    {
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
