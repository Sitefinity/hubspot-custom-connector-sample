namespace Telerik.Sitefinity.HubSpotConnector.Web.Services.DTO
{    
    /// <summary>
    /// Class that represent service stack request object.
    /// </summary>
    public class HubSpotFormsRequest
    {
        /// <summary>
        /// Gets or sets the searched text
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the number of returned results
        /// </summary>
        public int Take { get; set; }
    }
}