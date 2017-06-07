using System.Diagnostics.CodeAnalysis;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Telerik.Sitefinity.HubSpotConnector
{
    /// <summary>
    /// Localizable strings for the HubSpotConnector module
    /// </summary>
    [ObjectInfo("HubSpotConnectorResources", ResourceClassId = "HubSpotConnectorResources", Title = "HubSpotConnectorResourcesTitle", TitlePlural = "HubSpotConnectorResourcesTitlePlural", Description = "HubSpotConnectorResourcesDescription")]
    public class HubSpotConnectorResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public HubSpotConnectorResources()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider">Data provider <see cref="ResourceDataProvider"/></param>
        public HubSpotConnectorResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// Gets HubSpotConnector resources title
        /// </summary>
        /// <value>HubSpot connector labels</value>
        [ResourceEntry("HubSpotConnectorResourcesTitle",
            Value = "HubSpot connector labels",
            Description = "The title of this class.",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorResourcesTitle
        {
            get
            {
                return this["HubSpotConnectorResourcesTitle"];
            }
        }

        /// <summary>
        /// Gets HubSpotConnector resources title plural
        /// </summary>
        /// <value>HubSpot connector labels</value>
        [ResourceEntry("HubSpotConnectorResourcesTitlePlural",
            Value = "HubSpot connector labels",
            Description = "The title plural of this class.",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorResourcesTitlePlural
        {
            get
            {
                return this["HubSpotConnectorResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// Gets message: Contains localizable resources for HubSpotConnector module.
        /// </summary>
        /// <value>Contains localizable resources for HubSpot connector module.</value>
        [ResourceEntry("HubSpotConnectorResourcesDescription",
            Value = "Contains localizable resources for HubSpot connector module.",
            Description = "The description of this class.",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorResourcesDescription
        {
            get
            {
                return this["HubSpotConnectorResourcesDescription"];
            }
        }
        #endregion

        /// <summary>
        /// Gets phrase: Connector for HubSpot
        /// </summary>
        /// <value>Connector for HubSpot</value>
        [ResourceEntry("HubSpotConnectorGroupPageTitle",
            Value = "Connector for HubSpot",
            Description = "Phrase: Connector for HubSpot",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorGroupPageTitle
        {
            get
            {
                return this["HubSpotConnectorGroupPageTitle"];
            }
        }

        /// <summary>
        /// Gets word: HubSpot
        /// </summary>
        [ResourceEntry("HubSpotConnectorGroupPageUrlName",
            Value = "HubSpot",
            Description = "Word: HubSpot",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorGroupPageUrlName
        {
            get
            {
                return this["HubSpotConnectorGroupPageUrlName"];
            }
        }

        /// <summary>
        /// Gets phrase: Connector for HubSpot group page
        /// </summary>
        /// <value>Connector for HubSpot group page</value>
        [ResourceEntry("HubSpotConnectorGroupPageDescription",
            Value = "Connector for HubSpot group page",
            Description = "Phrase: Connector for HubSpot group page",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorGroupPageDescription
        {
            get
            {
                return this["HubSpotConnectorGroupPageDescription"];
            }
        }

        /// <summary>
        /// Gets phrase: Connector for HubSpot
        /// </summary>
        /// <value>Connector for HubSpot</value>
        [ResourceEntry("HubSpotConnectorPageTitle",
            Value = "Connector for HubSpot",
            Description = "Phrase: Connector for HubSpot",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorPageTitle
        {
            get
            {
                return this["HubSpotConnectorPageTitle"];
            }
        }

        /// <summary>
        /// Gets word: Settings
        /// </summary>
        [ResourceEntry("HubSpotConnectorPageUrlName",
            Value = "Settings",
            Description = "Word: Settings",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorPageUrlName
        {
            get
            {
                return this["HubSpotConnectorPageUrlName"];
            }
        }

        /// <summary>
        /// Gets message: Connector for HubSpot
        /// </summary>
        [ResourceEntry("HubSpotConnectorPageDescription",
            Value = "Connector for HubSpot",
            Description = "Connector for HubSpot",
            LastModified = "2017/04/10")]
        public string HubSpotConnectorPageDescription
        {
            get
            {
                return this["HubSpotConnectorPageDescription"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot form name
        /// </summary>
        /// <value>HubSpot form name</value>
        [ResourceEntry("HubSpotFormName",
            Value = "HubSpot form name",
            Description = "label: HubSpot form name",
            LastModified = "2017/04/19")]
        public string HubSpotFormName
        {
            get
            {
                return this["HubSpotFormName"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot settings
        /// </summary>
        /// <value>HubSpot settings</value>
        [ResourceEntry("HubSpotSettings",
            Value = "HubSpot settings",
            Description = "label: HubSpot settings",
            LastModified = "2017/05/04")]
        public string HubSpotSettings
        {
            get
            {
                return this["HubSpotSettings"];
            }
        }

        /// <summary>
        /// Gets label: Post data to HubSpot
        /// </summary>
        /// <value>Post data to HubSpot</value>
        [ResourceEntry("PostDataToHubSpot",
            Value = "Post data to HubSpot",
            Description = "label: Post data to HubSpot",
            LastModified = "2017/05/04")]
        public string PostDataToHubSpot
        {
            get
            {
                return this["PostDataToHubSpot"];
            }
        }

        /// <summary>
        /// Gets message: Form will post data to HubSpot once enabled in the Forms module. Make sure the form you have selected has the proper HubSpot settings there.
        /// </summary>
        /// <value>Form will post data to HubSpot once enabled in the Forms module. Make sure the form you have selected has the proper HubSpot settings there.</value>
        [ResourceEntry("HubSpotTabFormWidgetInfoText",
            Value = "Form will post data to HubSpot once enabled in the Forms module. Make sure the form you have selected has the proper HubSpot settings there.",
            Description = "message: Form will post data to HubSpot once enabled in the Forms module. Make sure the form you have selected has the proper HubSpot settings there. ",
            LastModified = "2017/05/11")]
        public string HubSpotTabFormWidgetInfoText
        {
            get
            {
                return this["HubSpotTabFormWidgetInfoText"];
            }
        }

        /// <summary>
        /// Gets text: Connect to HubSpot using your HubSpot credentials
        /// </summary>
        /// <value>Connect to HubSpot using your HubSpot credentials</value>
        [ResourceEntry("ConnectToHubSpotUsingYourHubSpotCredentials",
            Value = "Connect to HubSpot using your HubSpot credentials",
            Description = "text: Connect to HubSpot using your HubSpot credentials",
            LastModified = "2017/05/16")]
        public string ConnectToHubSpotUsingYourHubSpotCredentials
        {
            get
            {
                return this["ConnectToHubSpotUsingYourHubSpotCredentials"];
            }
        }

        /// <summary>
        /// Gets word: HubSpot
        /// </summary>
        /// <value>HubSpot text</value>
        [ResourceEntry("HubSpot",
            Value = "HubSpot",
            Description = "word: HubSpot",
            LastModified = "2017/05/16")]
        public string HubSpot
        {
            get
            {
                return this["HubSpot"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot Portal ID
        /// </summary>
        /// <value>HubSpot Portal ID</value>
        [ResourceEntry("HubSpotPortalId",
            Value = "HubSpot Portal ID",
            Description = "Gets label: HubSpot Portal ID",
            LastModified = "2017/05/22")]
        public string HubSpotPortalId
        {
            get
            {
                return this["HubSpotPortalId"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot API key
        /// </summary>
        /// <value>HubSpot API key</value>
        [ResourceEntry("HubSpotApiKey",
            Value = "HubSpot API key",
            Description = "Gets label: HubSpot API key",
            LastModified = "2017/05/22")]
        public string HubSpotApiKey
        {
            get
            {
                return this["HubSpotApiKey"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot API URL
        /// </summary>
        /// <value>HubSpot API URL</value>
        [ResourceEntry("HubSpotApiUrl",
            Value = "HubSpot API URL",
            Description = "Gets label: HubSpot API URL",
            LastModified = "2017/05/22")]
        public string HubSpotApiUrl
        {
            get
            {
                return this["HubSpotApiUrl"];
            }
        }

        /// <summary>
        /// Gets label: HubSpot form upload URL
        /// </summary>
        /// <value>HubSpot form upload URL</value>
        [ResourceEntry("HubSpotFormUploadUrl",
            Value = "HubSpot form upload URL",
            Description = "Gets label: HubSpot form upload URL",
            LastModified = "2017/05/22")]
        public string HubSpotFormUploadUrl
        {
            get
            {
                return this["HubSpotFormUploadUrl"];
            }
        }

        /// <summary>
        /// Gets word: Enabled
        /// </summary>
        /// <value>text - Enabled</value>
        [ResourceEntry("Enabled",
            Value = "Enabled",
            Description = "word: Enabled",
            LastModified = "2017/05/16")]
        public string Enabled
        {
            get
            {
                return this["Enabled"];
            }
        }

        /// <summary>
        /// Gets label: Autocomplete suggestions count
        /// </summary>
        /// <value>Autocomplete suggestions count</value>
        [ResourceEntry("AutocompleteSuggestionsCount",
            Value = "Autocomplete suggestions count",
            Description = "label: Autocomplete suggestions count",
            LastModified = "2017/05/22")]
        public string AutocompleteSuggestionsCount
        {
            get
            {
                return this["AutocompleteSuggestionsCount"];
            }
        }
    }
}