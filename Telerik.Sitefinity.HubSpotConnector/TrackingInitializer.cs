using Telerik.Sitefinity.HubSpotConnector.Web.UI;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.Events;

namespace Telerik.Sitefinity.HubSpotConnector
{
    /// <summary>
    /// Initializes the tracking scripts for HubSpot client side tracking.
    /// </summary>
    internal class TrackingInitializer
    {
        /// <summary>
        /// Initializes the tracking scripts for HubSpot client side tracking.
        /// </summary>
        public void Initialize()
        {
            EventHub.Unsubscribe<IPagePreRenderCompleteEvent>(evt => this.OnPagePreRenderCompleteEventHandler(evt));
            EventHub.Subscribe<IPagePreRenderCompleteEvent>(evt => this.OnPagePreRenderCompleteEventHandler(evt));
        }

        /// <summary>
        /// Uninitializes the tracking scripts for HubSpot client side tracking.
        /// </summary>
        public void Uninitialize()
        {
            EventHub.Unsubscribe<IPagePreRenderCompleteEvent>(evt => this.OnPagePreRenderCompleteEventHandler(evt));
        }

        /// <summary>
        /// Called when IPagePreRenderCompleteEvent event is fired.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        private void OnPagePreRenderCompleteEventHandler(IPagePreRenderCompleteEvent args)
        {
            if (this.ShouldRenderHubSpotScriptControl(args))
            {
                args.Page.Header.Controls.Add(new HubSpotScriptWidget());
            }
        }

        /// <summary>
        /// Returns whether the script control should be rendered on the page based on the provided <see cref="IPagePreRenderCompleteEvent"/> args.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        /// <returns>
        ///     <c>true</c> if the script control should be rendered; otherwise, <c>false</c>.
        /// </returns>
        private bool ShouldRenderHubSpotScriptControl(IPagePreRenderCompleteEvent args)
        {
            var module = SystemManager.GetModule(HubSpotConnectorModule.ModuleName);
            var moduleEnabled = module != null && SystemManager.CurrentContext.CurrentSite.IsModuleAccessible(module);

            bool shouldRenderHubSpotScriptControl = 
                moduleEnabled && !SystemManager.IsDesignMode && !SystemManager.IsPreviewMode && !args.PageSiteNode.IsBackend;

            return shouldRenderHubSpotScriptControl;
        }
    }
}