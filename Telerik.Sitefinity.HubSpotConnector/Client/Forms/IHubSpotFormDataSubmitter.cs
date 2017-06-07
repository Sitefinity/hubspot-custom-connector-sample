using System;
using System.Collections.Generic;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Exposes the interface of the HubSpot client API
    /// </summary>
    internal interface IHubSpotFormDataSubmitter : IDisposable
    {
        /// <summary>
        /// Submits the priovided form data to the HubSpot form with the provided Guid. 
        /// </summary>
        /// <param name="data">The list of names and values for the form fields.</param>
        /// <param name="formGuid">The Guid of the HubSpot form where the data should be posted.</param>
        void SubmitFormData(IEnumerable<KeyValuePair<string, string>> data, Guid formGuid);
    }
}