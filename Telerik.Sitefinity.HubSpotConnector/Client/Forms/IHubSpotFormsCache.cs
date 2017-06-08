using System;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <summary>
    /// Exposes the interface of HubSpot forms client API cache.
    /// </summary>
    internal interface IHubSpotFormsCache : IHubSpotFormsProvider, IDisposable
    {
    }
}