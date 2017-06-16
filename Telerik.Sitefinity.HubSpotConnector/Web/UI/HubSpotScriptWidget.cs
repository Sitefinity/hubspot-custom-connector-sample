using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Configuration;

namespace Telerik.Sitefinity.HubSpotConnector.Web.UI
{
    /// <summary>
    /// HubSpot tracking script widget
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class HubSpotScriptWidget : Control
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string portalId = Config.Get<HubSpotConnectorConfig>().HubSpotPortalId;
            if (!string.IsNullOrWhiteSpace(portalId))
            {
                this.Controls.Add(new LiteralControl(string.Format(HubSpotTrackingScriptFormat, portalId)));
            }
        }

        private const string HubSpotTrackingScriptFormat
            = "<script type=\"text/javascript\" id=\"hs-script-loader\" async defer src=\"//js.hs-scripts.com/{0}.js\"></script>";
    }
}