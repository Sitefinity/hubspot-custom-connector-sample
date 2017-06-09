using System;
using System.Collections.Generic;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.HubSpotConnector.Model;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Contains methods for retrieving cached HubSpot forms.
    /// </summary>
    internal class HubSpotFormsCache : IHubSpotFormsCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormsCache"/> class.
        /// </summary>
        public HubSpotFormsCache()
            : this(ObjectFactory.Resolve<IHubSpotFormsClient>(), SystemManager.GetCacheManager(CacheManagerInstance.Global))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormsCache"/> class.
        /// </summary>
        /// <param name="hubSpotFormsProvider">The <see cref="IHubSpotFormsProvider"/> implementation that will be used to get non-cached data.</param>
        internal HubSpotFormsCache(IHubSpotFormsProvider hubSpotFormsProvider, ICacheManager cacheManager)
        {
            this.hubSpotFormsProvider = hubSpotFormsProvider;
            this.cachedForms = new SynchronizedCache<IEnumerable<HubSpotForm>>(cacheManager);
        }

        /// <inheritdoc/>
        public IEnumerable<HubSpotForm> GetForms()
        {
            try
            {
                IEnumerable<HubSpotForm> forms = this.cachedForms.GetAndUpdateItem(HubSpotFormsCache.FormsCacheKey, () => this.hubSpotFormsProvider.GetForms());

                return forms;
            }
            catch (Exception ex)
            {
                Log.Write(ex);

                return null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed resources
        /// </summary>
        /// <param name="disposing">Defines whether a disposing is executing now.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.hubSpotFormsProvider != null)
                {
                    this.hubSpotFormsProvider.Dispose();
                }
            }
        }

        private readonly IHubSpotFormsProvider hubSpotFormsProvider;

        private readonly SynchronizedCache<IEnumerable<HubSpotForm>> cachedForms;

        private const string FormsCacheKey = "HubSpotForms";
    }
}