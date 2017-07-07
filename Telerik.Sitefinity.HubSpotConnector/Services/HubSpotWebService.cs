using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using ServiceStack;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Model;
using Telerik.Sitefinity.HubSpotConnector.Web.Services.DTO;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.ServiceStack.Filters;

namespace Telerik.Sitefinity.HubSpotConnector.Web.Services
{
    /// <summary>
    /// Class that represents the service stack service.
    /// </summary>
    public class HubSpotWebService : Service
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotWebService"/> class.
        /// </summary>
        public HubSpotWebService()
            : this(ObjectFactory.Resolve<IHubSpotFormsCache>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotWebService"/> class.
        /// </summary>
        /// <param name="hubSpotFormsProvider">The <see cref="IHubSpotFormsProvider"/> instance that will be used in the class.</param>
        internal HubSpotWebService(IHubSpotFormsProvider hubSpotFormsProvider)
        {
            this.hubSpotFormsProvider = hubSpotFormsProvider;
        }

        /// <summary>
        /// Method that returns suggestions for hubSpot forms.
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Returns suggestions for hubSpot forms.</returns>
        [AddHeader(ContentType = "application/json")]
        [RequestBackendAuthenticationFilterAttribute]
        public string[] Get(HubSpotFormsRequest request)
        {
            string[] result = new string[] { };
            var config = Config.Get<HubSpotConnectorConfig>();
            
            if (config.Enabled)
            {
                IEnumerable<HubSpotForm> forms = this.hubSpotFormsProvider.GetForms();
                if (forms != null)
                {
                    if (!string.IsNullOrEmpty(request.Term))
                    {
                        forms = forms.Where(f => f.Name.IndexOf(request.Term, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    result = forms.Select(f => f.Name).Take(request.Take).ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Method that saves HubSpot configuration.
        /// </summary>
        /// <param name="request">Request object</param>
        [AddHeader(ContentType = "application/json")]
        [RequestAdministrationAuthenticationFilter]
        public void Post(HubSpotConfigurationRequest request)
        {
            try
            {
                HubSpotConnectorConfig hubSpotConnectorConfig = new HubSpotConnectorConfig();
                hubSpotConnectorConfig.HubSpotPortalId = request.PortalId;
                hubSpotConnectorConfig.HubSpotApiKey = request.ApiKey;

                using (var testConnectionClient = new HubSpotFormsClient(hubSpotConnectorConfig, new HttpClient(), new FormUrlBuilder()))
                {
                    testConnectionClient.GetForms();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, TraceEventType.Error);

                throw new Exception(Res.Get<Labels>().UnableToConnectCheckYourCredentials);
            }

            var configManager = ConfigManager.GetManager();
            var config = configManager.GetSection<HubSpotConnectorConfig>();

            if (config.HubSpotPortalId != request.PortalId || config.HubSpotApiKey != request.ApiKey)
            {
                config.Enabled = true;
                config.HubSpotPortalId = request.PortalId;
                config.HubSpotApiKey = request.ApiKey;

                configManager.SaveSection(config, true);
            }
        }

        /// <summary>
        /// Changes the state of HubSpot module - enabled/disabled
        /// </summary>
        /// <param name="request">The state</param>
        /// <returns>The module state</returns>
        [AddHeader(ContentType = "application/json")]
        [RequestAdministrationAuthenticationFilter]
        public bool Post(bool request)
        {
            var configManager = ConfigManager.GetManager();
            var config = configManager.GetSection<HubSpotConnectorConfig>();

            config.Enabled = request;

            configManager.SaveSection(config, true);

            return request;
        }

        private readonly IHubSpotFormsProvider hubSpotFormsProvider;
    }
}