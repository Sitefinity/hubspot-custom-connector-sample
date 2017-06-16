using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Represents a HubSpot forms client
    /// </summary>
    internal class HubSpotFormsClient : IHubSpotFormsClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormsClient"/> class.
        /// </summary>
        public HubSpotFormsClient()
            : this(Config.Get<HubSpotConnectorConfig>(), new HttpClient(), new FormUrlBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotFormsClient"/> class.
        /// </summary>
        /// <param name="hubSpotConnectorConfig">The HubSpot connector configuration</param>
        /// <param name="httpClient">The http client used</param>
        /// <param name="formUrlBuilder">The form URL builder</param>
        internal HubSpotFormsClient(HubSpotConnectorConfig hubSpotConnectorConfig, HttpClient httpClient, IFormUrlBuilder formUrlBuilder)
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

            this.apiKey = hubSpotConnectorConfig.HubSpotApiKey;
            this.formUrlBuilder = formUrlBuilder;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(hubSpotConnectorConfig.HubSpotApiUrl, UriKind.Absolute);
            this.DecorateHeaders(this.httpClient, HubSpotFormsClient.AcceptContentTypeHeaderValue);
        }

        /// <inheritdoc/>
        public IEnumerable<HubSpotForm> GetForms()
        {
            string url = this.formUrlBuilder.BuildFormsUrl(this.apiKey);

            HttpResponseMessage httpResponseMessage = this.httpClient.GetAsync(url).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            IEnumerable<HubSpotForm> formsList = null;

            try
            {
                formsList = JsonConvert.DeserializeObject<IEnumerable<HubSpotForm>>(responseContent);
            }
            catch (Exception ex)
            {
                Log.Write(responseContent);
                throw ex;
            }

            return formsList;
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

        private void DecorateHeaders(HttpClient client, string acceptContentTypeHeaderValue)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentTypeHeaderValue));
        }

        private readonly IFormUrlBuilder formUrlBuilder;
        private readonly HttpClient httpClient;

        private readonly string apiKey;

        private const string AcceptContentTypeHeaderValue = "application/json";
    }
}