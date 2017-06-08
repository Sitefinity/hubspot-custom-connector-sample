using System;
using System.Collections.Generic;
using System.Net.Http;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Configuration;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Represents a HubSpot forms client
    /// </summary>
    internal class HubSpotFormDataSubmitter : IHubSpotFormDataSubmitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormDataSubmitter"/> class.
        /// </summary>
        [InjectionConstructor]
        public HubSpotFormDataSubmitter()
            : this(Config.Get<HubSpotConnectorConfig>(), new HttpClient(), new FormUrlBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormDataSubmitter"/> class.
        /// </summary>
        /// <param name="hubSpotConnectorConfig">The HubSpot connector configuration</param>
        /// <param name="httpClient">The http client used</param>
        /// <param name="formUrlBuilder">The form URL builder</param>
        internal HubSpotFormDataSubmitter(HubSpotConnectorConfig hubSpotConnectorConfig, HttpClient httpClient, IFormUrlBuilder formUrlBuilder)
        {
            if (hubSpotConnectorConfig == null)
            {
                throw new ArgumentNullException("hubSpotConnectorConfig");
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            if (formUrlBuilder == null)
            {
                throw new ArgumentNullException("formUrlBuilder");
            }

            this.portalId = hubSpotConnectorConfig.HubSpotPortalId;
            this.formUrlBuilder = formUrlBuilder;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(hubSpotConnectorConfig.HubSpotFormUploadUrl, UriKind.Absolute);
        }

        /// <inheritdoc/>
        public void SubmitFormData(IEnumerable<KeyValuePair<string, string>> data, Guid formGuid)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string url = this.formUrlBuilder.BuildSubmitFormUrl(this.portalId, formGuid);
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(data);

            HttpResponseMessage httpResponseMessage = this.httpClient.PostAsync(url, formUrlEncodedContent).Result;
            httpResponseMessage.EnsureSuccessStatusCode();
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
                if (this.httpClient != null)
                {
                    this.httpClient.Dispose();
                }
            }
        }

        private readonly IFormUrlBuilder formUrlBuilder;
        private readonly HttpClient httpClient;

        private readonly string portalId;

        private const string AcceptContentTypeHeaderValue = "application/json";
    }
}