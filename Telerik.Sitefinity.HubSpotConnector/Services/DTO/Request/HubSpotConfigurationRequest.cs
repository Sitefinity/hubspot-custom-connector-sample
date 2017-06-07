namespace Telerik.Sitefinity.HubSpotConnector.Web.Services.DTO
{    
    /// <summary>
    /// Class that represent service stack request object.
    /// </summary>
    public class HubSpotConfigurationRequest
    {
        /// <summary>
        /// Gets or sets the portal id
        /// </summary>
        public string PortalId { get; set; }

        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public string ApiKey { get; set; }
    }
}