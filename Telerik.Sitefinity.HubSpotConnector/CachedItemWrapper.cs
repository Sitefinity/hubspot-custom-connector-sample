using System;

namespace Telerik.Sitefinity.HubSpotConnector
{
    /// <summary>
    /// The class wraps the item that need to be cached and the timestamp when it is added to the cache.
    /// </summary>
    /// <typeparam name="T">The type of cached items</typeparam>
    internal class CachedItemWrapper<T> where T : class
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; set; }

        /// <summary>
        /// Gets or sets the caching timestamp.
        /// </summary>
        /// <value>
        /// The added to cache time stamp.
        /// </value>
        public DateTime AddedToCacheTimeStamp { get; set; }

        public bool NeedsRefresh(int minutesThreshold)
        {
            TimeSpan elapsed = DateTime.UtcNow - this.AddedToCacheTimeStamp;
            return elapsed.TotalMinutes > minutesThreshold;
        }
    }
}