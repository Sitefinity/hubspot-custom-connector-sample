using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Abstractions.VirtualPath.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Configuration.Data;
using Telerik.Sitefinity.Connectivity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Forms;
using Telerik.Sitefinity.HubSpotConnector.Web.Services;
using Telerik.Sitefinity.HubSpotConnector.Web.UI;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.HubSpotConnector
{
    /// <summary>
    /// HubSpot connector module
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HubSpotConnectorModule : ModuleBase
    {
        /// <inheritdoc />
        public override Guid LandingPageId
        {
            get
            {
                return PageId;
            }
        }

        /// <inheritdoc />
        public override void Initialize(ModuleSettings settings)
        {
            App.WorkWith()
                .Module(HubSpotConnectorModule.ModuleName)
                .Initialize()
                .Localization<HubSpotConnectorResources>()
                .Configuration<HubSpotConnectorConfig>()
                .ServiceStackPlugin(new HubSpotServiceStackPlugin());

            base.Initialize(settings);
            this.RegisterIocTypes();
        }

        /// <summary>
        /// Integrate the module into the system.
        /// </summary>
        public override void Load()
        {
            Bootstrapper.Initialized -= this.Bootstrapper_Initialized;
            Bootstrapper.Initialized += this.Bootstrapper_Initialized;
        }

        /// <summary>
        /// Handles the Initialized event of the Bootstrapper.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Sitefinity.Data.ExecutedEventArgs"/> instance containing the event data.</param>
        protected virtual void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped" && SystemManager.GetModule(HubSpotConnectorModule.ModuleName) != null)
            {
                var configManager = ConfigManager.GetManager();
                configManager.Provider.Executed += this.ConfigEventHandler;

                if (this.HubSpotConfigHasRequiredSettings())
                {
                    this.InitializeTrackingInitializer();
                    this.InitializeFormDataSender();
                    this.InitializeHubSpotFormsCache();
                }
            }
        }
        
        /// <summary>
        /// This method is invoked during the unload process of an active module from Sitefinity, e.g. when a module is deactivated. For instance this method is also invoked for every active module during a restart of the application. 
        /// Typically you will use this method to unsubscribe the module from all events to which it has subscription.
        /// </summary>
        public override void Unload()
        {
            this.UninitializeTrackingInitializer();
            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            var configManager = ConfigManager.GetManager();
            configManager.Provider.Executed -= this.ConfigEventHandler;

            Bootstrapper.Initialized -= this.Bootstrapper_Initialized;

            base.Unload();
        }

        /// <summary>
        /// Uninstall the module from Sitefinity system. Deletes the module artifacts added with Install method.
        /// </summary>
        /// <param name="initializer">The site initializer instance.</param>
        public override void Uninstall(SiteInitializer initializer)
        {
            this.UninitializeTrackingInitializer();
            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            var configManager = ConfigManager.GetManager();
            configManager.Provider.Executed -= this.ConfigEventHandler;

            base.Uninstall(initializer);
        }

        /// <inheritdoc />
        public override void Install(Abstractions.SiteInitializer initializer)
        {
            ConnectorsHelper.CreateConnectivityGroupPage(initializer);

            initializer.Installer
                .CreateModuleGroupPage(ModuleGroupPageId, "HubSpotConnectorGroupPage")
                    .PlaceUnder(SiteInitializer.ConnectivityPageNodeId)
                    .SetOrdinal(7)
                    .LocalizeUsing<HubSpotConnectorResources>()
                    .SetTitleLocalized("HubSpotConnectorGroupPageTitle")
                    .SetUrlNameLocalized("HubSpotConnectorGroupPageUrlName")
                    .SetDescriptionLocalized("HubSpotConnectorGroupPageDescription")
                    .ShowInNavigation()
                    .AddChildPage(PageId, "HubSpotConnectorPage")
                        .SetOrdinal(1)
                        .LocalizeUsing<HubSpotConnectorResources>()
                        .SetTitleLocalized("HubSpotConnectorPageTitle")
                        .SetHtmlTitleLocalized("HubSpotConnectorPageTitle")
                        .SetUrlNameLocalized("HubSpotConnectorPageUrlName")
                        .SetDescriptionLocalized("HubSpotConnectorPageDescription")
                        .AddControl(new HubSpotConnectorSettings())
                        .HideFromNavigation()
                    .Done()
                .Done();
        }

        /// <inheritdoc />
        protected override ConfigSection GetModuleConfig()
        {
            return Config.Get<HubSpotConnectorConfig>();
        }

        /// <inheritdoc />
        protected override IDictionary<string, Action<VirtualPathElement>> GetVirtualPaths()
        {
            var paths = new Dictionary<string, Action<VirtualPathElement>>();
            paths.Add(ModuleVirtualPath + "*", null);
            return paths;
        }

        /// <inheritdoc />
        public override Type[] Managers
        {
            get
            {
                return new Type[0];
            }
        }

        /// <summary>
        /// Handles the event for config update.
        /// </summary>
        /// <param name="configEvent">The config change event args.</param>
        private void ConfigEventHandler(object sender, ExecutedEventArgs e)
        {
            bool isHubSpotConfigUpdated = e.CommandArguments is HubSpotConnectorConfig;

            if (!isHubSpotConfigUpdated)
            {
                return;
            }

            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            if (this.HubSpotConfigHasRequiredSettings())
            {
                this.InitializeTrackingInitializer();
                this.InitializeFormDataSender();
                this.InitializeHubSpotFormsCache();
            }
        }

        /// <summary>
        /// Uninitializes the local <see cref="TrackingInitializer"/>.
        /// </summary>
        private void InitializeTrackingInitializer()
        {
            if (this.trackingInitializer == null)
            {
                this.trackingInitializer = new TrackingInitializer();
            }

            this.trackingInitializer.Initialize();
        }

        /// <summary>
        /// Uninitialize the local <see cref="TrackingInitializer"/>.
        /// </summary>
        private void UninitializeTrackingInitializer()
        {
            if (this.trackingInitializer != null)
            {
                this.trackingInitializer.Uninitialize();
            }
        }

        /// <summary>
        /// Initializes the local <see cref="HubSpotConnectorFormDataSender"/>.
        /// </summary>
        private void InitializeFormDataSender()
        {
            this.connectorFormDataSender = new HubSpotConnectorFormDataSender();
            ConnectorFormsEventHandler.RegisterSender(this.connectorFormDataSender);
        }

        /// <summary>
        /// Initializes the forms cache and re initializes the IHubSpotFormsClient
        /// </summary>
        private void InitializeHubSpotFormsCache()
        {
            SystemManager.BackgroundTasksService.EnqueueTask(() =>
            {
                IHubSpotFormsCache hubSpotFormsCache = ObjectFactory.Resolve<IHubSpotFormsCache>();
                hubSpotFormsCache.GetForms();
            });
        }

        /// <summary>
        /// Disposes the local <see cref="HubSpotConnectorFormDataSender"/>.
        /// </summary>
        private void DisposeFormDataSender()
        {
            if (this.connectorFormDataSender != null)
            {
                ConnectorFormsEventHandler.UnregisterSender(this.connectorFormDataSender);
                this.connectorFormDataSender.Dispose();
                this.connectorFormDataSender = null;
            }
        }

        /// <summary>
        /// Disposes all singleton instances that have <see cref="ContainerControlledLifetimeManager"/> registered in the local
        /// containerControlledLifetimeManagers field.
        /// </summary>
        private void DisposeSingletonInstances()
        {
            foreach (ContainerControlledLifetimeManager containerControlledLifetimeManager in this.containerControlledLifetimeManagers)
            {
                containerControlledLifetimeManager.RemoveValue();
            }
        }

        /// <summary>
        /// Checks whether the HubSpot config has the required settings for the connector to work.
        /// </summary>
        /// <returns>Returns true if the config has the required settings for the connector. Otherwise, false.</returns>
        private bool HubSpotConfigHasRequiredSettings()
        {
            HubSpotConnectorConfig hubSpotConnectorConfig = Config.Get<HubSpotConnectorConfig>();

            return this.HubSpotConfigHasRequiredSettings(hubSpotConnectorConfig);
        }

        /// <summary>
        /// Checks whether the HubSpot config has the required settings for the connector to work.
        /// </summary>
        /// <param name="hubSpotConnectorConfig">The HubSpot config object.</param>
        /// <returns>Returns true if the config has the required settings for the connector. Otherwise, false.</returns>
        private bool HubSpotConfigHasRequiredSettings(HubSpotConnectorConfig hubSpotConnectorConfig)
        {
            if (string.IsNullOrWhiteSpace(hubSpotConnectorConfig.HubSpotPortalId))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(hubSpotConnectorConfig.HubSpotApiKey))
            {
                return false;
            }

            if (!hubSpotConnectorConfig.Enabled)
            {
                return false;
            }

            return true;
        }

        private void RegisterIocTypes()
        {
            ObjectFactory.Container.RegisterType<FormsConnectorDesignerExtender, HubSpotFormsConnectorDesignerExtender>(HubSpotConnectorModule.ModuleName);
            ObjectFactory.Container.RegisterType<FormsConnectorDefinitionsExtender, HubSpotFormsConnectorDefinitionsExtender>(HubSpotConnectorModule.ModuleName);
            ObjectFactory.Container.RegisterType<ConnectorDataMappingExtender, HubSpotConnectorDataMappingExtender>(HubSpotConnectorModule.ModuleName);

            ContainerControlledLifetimeManager hubSpotFormsClientLifetimeManager = new ContainerControlledLifetimeManager();
            ObjectFactory.Container.RegisterType<IHubSpotFormsClient, HubSpotFormsClient>(hubSpotFormsClientLifetimeManager);
            this.containerControlledLifetimeManagers.Add(hubSpotFormsClientLifetimeManager);

            ContainerControlledLifetimeManager hubSpotFormsCacheLifetimeManager = new ContainerControlledLifetimeManager();
            ObjectFactory.Container.RegisterType<IHubSpotFormsCache, HubSpotFormsCache>(hubSpotFormsCacheLifetimeManager);
            this.containerControlledLifetimeManagers.Add(hubSpotFormsCacheLifetimeManager);

            ContainerControlledLifetimeManager hubSpotFormSubmitterLifetimeManager = new ContainerControlledLifetimeManager();
            ObjectFactory.Container.RegisterType<IHubSpotFormDataSubmitter, HubSpotFormDataSubmitter>(hubSpotFormSubmitterLifetimeManager);
            this.containerControlledLifetimeManagers.Add(hubSpotFormSubmitterLifetimeManager);
        }

        /// <summary>
        /// The name of this module
        /// </summary>
        public const string ModuleName = "HubSpotConnector";

        internal static readonly Guid ModuleGroupPageId = new Guid("85F7603C-8663-4CBF-BD8F-BEC0734A941F");
        internal static readonly Guid PageId = new Guid("B7A27F64-96ED-49CB-AEC9-09A2364A4E98");

        internal const string ModuleVirtualPath = "~/HubSpotConnector/";
        private const string HubSpotConnectorConfigName = "HubSpotConnectorConfig";

        private HubSpotConnectorFormDataSender connectorFormDataSender;
        private TrackingInitializer trackingInitializer;
        private IList<ContainerControlledLifetimeManager> containerControlledLifetimeManagers = new List<ContainerControlledLifetimeManager>();
    }
}