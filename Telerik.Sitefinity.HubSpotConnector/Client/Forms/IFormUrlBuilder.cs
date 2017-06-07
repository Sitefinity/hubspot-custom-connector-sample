using System;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Builds form URLs for HubSpot REST API calls.
    /// </summary>
    internal interface IFormUrlBuilder
    {
        /// <summary>
        /// Builds a URL that is used to call the REST API form multiple forms.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The URL path and query.</returns>
        string BuildFormsUrl(string apiKey = null);

        /// <summary>
        /// Builds a URL that is used to submit data to a form.
        /// </summary>
        /// <param name="portalId">The portal ID.</param>
        /// <param name="formGuid">The form Guid.</param>
        /// <returns>The URL path and query.</returns>
        string BuildSubmitFormUrl(string portalId, Guid formGuid);
    }
}