using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Web.Services;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields.Config;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace Telerik.Sitefinity.HubSpotConnector.Forms
{
    /// <summary>
    /// Extends forms definitions by adding HubSpot specific fields in the form properties.
    /// </summary>
    internal class HubSpotFormsConnectorDefinitionsExtender : FormsConnectorDefinitionsExtender
    {
        /// <inheritdoc/>
        public override int Ordinal
        {
            get
            {
                return 3;
            }
        }

        /// <inheritdoc/>
        public override void AddConnectorSettings(ConfigElementDictionary<string, FieldDefinitionElement> sectionFields)
        {
            var hubSpotFormGuidField = new TextFieldDefinitionElement(sectionFields)
            {   
                Title = HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName,
                DataFieldName = string.Format("{0}.{1}", FormAttributesPropertyName, HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName),
                DisplayMode = FieldDisplayMode.Write,
                FieldName = HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName,
                CssClass = HubSpotFormsConnectorDefinitionsExtender.DependentControlsCssClass,
                FieldType = typeof(TextField),
                ID = HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName,
                ResourceClassId = ResourceClassId,
                AutocompleteServiceUrl = string.Concat("/restapi", HubSpotServiceStackPlugin.FormsRoute, "?take={take}"),
                AutocompleteSuggestionsCount = Config.Get<HubSpotConnectorConfig>().AutocompleteSuggestionsCount
            };

            sectionFields.Add(hubSpotFormGuidField);
        }

        private const string FormAttributesPropertyName = "Attributes";
        public const string HubSpotFormNameFieldName = "HubSpotFormName";
        public const string DependentControlsCssClass = "sfHubSpotDependentCtrls";
        public static readonly string ResourceClassId = typeof(HubSpotConnectorResources).Name;
    }
}