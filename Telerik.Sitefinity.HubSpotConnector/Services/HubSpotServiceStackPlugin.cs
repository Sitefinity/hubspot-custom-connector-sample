using ServiceStack;
using System;
using Telerik.Sitefinity.HubSpotConnector.Web.Services.DTO;

namespace Telerik.Sitefinity.HubSpotConnector.Web.Services
{
    /// <summary>
    /// Represents a HubSpot plug-in for the search web service.
    /// </summary>
    internal class HubSpotServiceStackPlugin : IPlugin
    {
        /// <summary>
        /// Adding the service routes
        /// </summary>
        /// <param name="appHost">The service stack appHost</param>
        public void Register(IAppHost appHost)
        {
            if (appHost == null)
                throw new ArgumentNullException("appHost");

            appHost.RegisterService<HubSpotWebService>();
            appHost.Routes
                   .Add<HubSpotFormsRequest>(FormsRoute, "GET")
                   .Add<HubSpotConfigurationRequest>(ConfigurationRoute, "POST")
                   .Add<bool>(ModuleStatusRoute, "POST");
        }

        internal static readonly string FormsRoute = string.Concat(ServiceRoute, "/forms");
        internal static readonly string ConfigurationRoute = string.Concat(ServiceRoute, "/config");
        internal static readonly string ModuleStatusRoute = string.Concat(ServiceRoute, "/status");
        private const string ServiceRoute = "/hubspot";
    }
}