using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Model;
using Telerik.Sitefinity.Modules.Forms;

namespace Telerik.Sitefinity.HubSpotConnector.Forms
{
    /// <summary>
    /// The class defines data mapping between Sitefinity and HubSpot connectors' fields.
    /// </summary>
    internal class HubSpotConnectorDataMappingExtender : ConnectorDataMappingExtender
    {
        /// <inheritdoc />
        public override string Key
        {
            get 
            {
                return HubSpotConnectorModule.ModuleName;
            }
        }

        /// <inheritdoc />
        public override string DependentControlsCssClass
        {
            get
            {
                return HubSpotFormsConnectorDefinitionsExtender.DependentControlsCssClass;
            }
        }

        /// <inheritdoc />
        public override string AutocompleteRequiredControlsCssClass
        {
            get
            {
                return HubSpotFormsConnectorDefinitionsExtender.DependentControlsCssClass;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorDataMappingExtender"/> class.
        /// </summary>
        public HubSpotConnectorDataMappingExtender()
            : this(ObjectFactory.Resolve<IHubSpotFormsCache>(), Config.Get<HubSpotConnectorConfig>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorDataMappingExtender"/> class.
        /// </summary>
        /// <param name="hubSpotFormsProvider">The <see cref="IHubSpotFormsProvider"/> instance that will be used in the class.</param>
        /// <param name="hubSpotConnectorConfig">The <see cref="HubSpotConnectorConfig"/> instance that will be used in the class.</param>
        internal HubSpotConnectorDataMappingExtender(IHubSpotFormsProvider hubSpotFormsProvider, HubSpotConnectorConfig hubSpotConnectorConfig)
        {
            this.hubSpotFormsProvider = hubSpotFormsProvider;
            this.hubSpotConnectorConfig = hubSpotConnectorConfig;
        }

        /// <inheritdoc />
        public override IEnumerable<string> GetAutocompleteData(string term, string[] paramValues)
        {
            if (!this.hubSpotConnectorConfig.Enabled || !paramValues.Any())
            {
                return null;
            }

            string formName = paramValues[0];
            if (string.IsNullOrWhiteSpace(formName))
            {
                return null;
            }

            IEnumerable<HubSpotForm> forms = this.hubSpotFormsProvider.GetForms();
            if (forms == null || !forms.Any())
            {
                return null;
            }

            HubSpotForm form = forms.FirstOrDefault(f => f.Name == formName);
            if (form == null || form.FormFieldGroups == null || !form.FormFieldGroups.Any())
            {
                return null;
            }

            IEnumerable<HubSpotFormField> result = form.FormFieldGroups.SelectMany(g => g.Fields);
            if (!string.IsNullOrWhiteSpace(term))
            {
                result = result.Where(f => f != null && f.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            int take = this.hubSpotConnectorConfig.AutocompleteSuggestionsCount;
            if (take > 0)
            {
                result = result.Take(take);
            }

            return result.Select(r => r.Name);
        }

        /// <inheritdoc />
        public override bool HasAutocomplete
        {
            get
            {
                return true;
            }
        }

        private readonly IHubSpotFormsProvider hubSpotFormsProvider;
        private readonly HubSpotConnectorConfig hubSpotConnectorConfig;
    }
}