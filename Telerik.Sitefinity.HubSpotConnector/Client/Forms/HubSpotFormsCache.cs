using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.HubSpotConnector.Model;

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
            : this(ObjectFactory.Resolve<IHubSpotFormsClient>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormsCache"/> class.
        /// </summary>
        /// <param name="hubSpotFormsClient">The <see cref="IHubSpotFormsClient"/> implementation that will be used to get non-cached data.</param>
        internal HubSpotFormsCache(IHubSpotFormsProvider hubSpotFormsProvider)
        {
            this.hubSpotFormsProvider = hubSpotFormsProvider;
            this.cachedForms = new SynchronizedCache<IEnumerable<HubSpotForm>>();
        }

        /// <inheritdoc/>
        public IEnumerable<HubSpotForm> GetForms()
        {
            try
            {
                IEnumerable<HubSpotForm> forms = cachedForms.GetAndUpdateItem(HubSpotFormsCache.FormsCacheKey, () => this.hubSpotFormsProvider.GetForms());

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