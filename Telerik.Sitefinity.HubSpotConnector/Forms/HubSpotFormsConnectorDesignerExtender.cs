using System.Collections.Generic;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Forms;

namespace Telerik.Sitefinity.HubSpotConnector.Forms
{
    /// <summary>
    /// Extends the designer of the Forms widget by adding new settings for HubSpot. 
    /// </summary>
    internal class HubSpotFormsConnectorDesignerExtender : FormsConnectorDesignerExtender
    {
        /// <inheritdoc/>
        public override string Title
        {
            get
            {
                return HubSpotConnectorModule.ModuleName;
            }
        }

        /// <inheritdoc/>
        public override string Name
        {
            get
            {
                return HubSpotConnectorModule.ModuleName;
            }
        }

        /// <inheritdoc/>
        public override IList<PropertyDescription> GetProperties()
        {
            return new List<PropertyDescription>()
            {
                new PropertyDescription()
                {
                    Type = PropertyDescription.PropertyType.Bool,
                    Name = HubSpotFormsConnectorDesignerExtender.PostDataToHubSpotPropertyName,
                    Title = Res.Get<HubSpotConnectorResources>().HubSpotSettings,
                    Text = Res.Get<HubSpotConnectorResources>().PostDataToHubSpot
                }
            };
        }

        public const string PostDataToHubSpotPropertyName = "PostDataToHubSpot";
    }
}