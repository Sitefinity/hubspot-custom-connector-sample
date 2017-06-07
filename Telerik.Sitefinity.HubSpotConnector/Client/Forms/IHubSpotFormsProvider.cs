using System;
using System.Collections.Generic;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Exposes the interface of the HubSpot client API.
    /// </summary>
    internal interface IHubSpotFormsProvider : IDisposable
    {
        /// <summary>
        /// Gets all HubSpot forms.
        /// </summary>
        /// <returns>The forms list.</returns>
        IEnumerable<HubSpotForm> GetForms();
    }
}