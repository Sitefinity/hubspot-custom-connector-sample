using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Web.UI;

namespace Telerik.Sitefinity.HubSpotConnector.Web.UI
{
    /// <summary>
    /// HubSpot connector view
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HubSpotConnectorSettings : SimpleScriptView
    {
        /// <inheritdoc />
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return HubSpotConnectorSettings.LayoutPath;
                return base.LayoutTemplatePath;
            }

            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        #region Controls

        /// <summary>
        /// Gets the siteId box.
        /// </summary>
        /// <value>The box.</value>
        protected virtual TextBox PortalIdTextBox
        {
            get
            {
                return this.Container.GetControl<TextBox>("portalIdTextBox", true);
            }
        }

        /// <summary>
        /// Gets the box with user's name.
        /// </summary>
        /// <value>The box.</value>
        protected virtual TextBox ApiKeyTextBox
        {
            get
            {
                return this.Container.GetControl<TextBox>("apiKeyTextBox", true);
            }
        }

        /// <summary>
        /// Gets the connect button.
        /// </summary>
        /// <value>The connect button.</value>
        protected virtual LinkButton ConnectButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("connectButton", true);
            }
        }

        /// <summary>
        /// Gets the change connection button.
        /// </summary>
        /// <value>The button.</value>
        protected virtual LinkButton ChangeConnectionButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("changeConnectionButton", true);
            }
        }

        /// <summary>
        /// Gets the disconnect reconnect button.
        /// </summary>
        /// <value>The button.</value>
        protected virtual LinkButton DisconnectReconnectButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("disconnectReconnectButton", true);
            }
        }

        /// <summary>
        /// Gets the error message control.
        /// </summary>
        /// <value>The control.</value>
        protected virtual HtmlControl ErrorMessageWrapper
        {
            get
            {
                return this.Container.GetControl<HtmlControl>("errorMessageWrapper", true);
            }
        }
        
        /// <summary>
        /// Gets the loading control.
        /// </summary>
        /// <value>The control.</value>
        protected virtual HtmlControl LoadingView
        {
            get
            {
                return this.Container.GetControl<HtmlControl>("loadingView", true);
            }
        }

        #endregion

        /// <inheritdoc />
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptor = new ScriptControlDescriptor(this.GetType().FullName, this.ClientID);

            descriptor.AddProperty("_hubSpotModuleEnabled", Config.Get<HubSpotConnectorConfig>().Enabled);
            descriptor.AddProperty("_connectText", Res.Get<Labels>().Connect);
            descriptor.AddProperty("_disconnectText", Res.Get<Labels>().Disconnect);

            descriptor.AddElementProperty("portalIdTextBox", this.PortalIdTextBox.ClientID);
            descriptor.AddElementProperty("apiKeyTextBox", this.ApiKeyTextBox.ClientID);

            descriptor.AddElementProperty("connectButton", this.ConnectButton.ClientID);
            descriptor.AddElementProperty("changeConnectionButton", this.ChangeConnectionButton.ClientID);
            descriptor.AddElementProperty("disconnectReconnectButton", this.DisconnectReconnectButton.ClientID);

            descriptor.AddProperty("_errorMessageWrapperId", this.ErrorMessageWrapper.ClientID);
            descriptor.AddProperty("_loadingViewId", this.LoadingView.ClientID);

            return new[] { descriptor };
        }

        /// <inheritdoc />
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>();
            var assemblyName = this.GetType().Assembly.FullName;

            scripts.Add(new ScriptReference("Telerik.Sitefinity.Web.Scripts.ClientManager.js", "Telerik.Sitefinity"));
            scripts.Add(new ScriptReference(HubSpotConnectorSettings.HubSpotConnectorSettingsScript, assemblyName));

            return scripts;
        }

        /// <inheritdoc />
        protected override void InitializeControls(GenericContainer container)
        {
            var config = Config.Get<HubSpotConnectorConfig>();
            this.PortalIdTextBox.Text = config.HubSpotPortalId;
            this.ApiKeyTextBox.Text = config.HubSpotApiKey;
        }

        private static readonly string LayoutPath = string.Concat(HubSpotConnectorModule.ModuleVirtualPath, "Telerik.Sitefinity.HubSpotConnector.Web.Views.HubSpotConnectorSettings.ascx");
        private const string HubSpotConnectorSettingsScript = "Telerik.Sitefinity.HubSpotConnector.Web.Scripts.HubSpotConnectorSettings.js";
    }
}