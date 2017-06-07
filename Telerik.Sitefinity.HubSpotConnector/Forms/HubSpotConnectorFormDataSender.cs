using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.HubSpotConnector.Forms
{
    /// <summary>
    /// Used for sending form data for the HubSpot connector integration.
    /// </summary>
    internal class HubSpotConnectorFormDataSender : IConnectorFormDataSender
    {
        /// <inheritdoc/>
        public string DataMappingExtenderKey
        {
            get
            {
                return HubSpotConnectorModule.ModuleName;
            }
        }

        /// <inheritdoc/>
        public string DesignerExtenderName
        {
            get
            {
                return HubSpotConnectorModule.ModuleName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorFormDataSender"/> class.
        /// </summary>
        public HubSpotConnectorFormDataSender()
            : this(ObjectFactory.Resolve<IHubSpotFormDataSubmitter>(), ObjectFactory.Resolve<HubSpotFormsCache>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotConnectorFormDataSender"/> class.
        /// </summary>
        /// <param name="hubSpotFormDataSubmitter">The <see cref="IHubSpotFormDataSubmitter"/> instance that will be used in the class.</param>
        /// <param name="hubSpotFormsProvider">The <see cref="IHubSpotFormsProvider"/> instance that will be used in the class.</param>
        internal HubSpotConnectorFormDataSender(IHubSpotFormDataSubmitter hubSpotFormDataSubmitter, IHubSpotFormsProvider hubSpotFormsProvider)
        {
            if (hubSpotFormDataSubmitter == null)
            {
                throw new ArgumentNullException("hubSpotFormDataSubmitter");
            }

            this.hubSpotFormDataSubmitter = hubSpotFormDataSubmitter;
            this.hubSpotFormsProvider = hubSpotFormsProvider;
        }

        /// <inheritdoc/>
        public bool ShouldSendFormData(ConnectorFormDataContext dataContext)
        {
            bool shouldPostDataToHubSpot = bool.Parse(dataContext.WidgetDesignerSettings[HubSpotFormsConnectorDesignerExtender.PostDataToHubSpotPropertyName]);
            if (!shouldPostDataToHubSpot)
            {
                return false;
            }

            string formName = dataContext.FormDescriptionAttributeSettings[HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName];
            if (string.IsNullOrWhiteSpace(formName))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public void SendFormData(IDictionary<string, string> data, ConnectorFormDataContext dataContext)
        {
            try
            {
                Guid formGuid = this.GetFormGuid(dataContext);

                data = this.AddHubSpotContext(data, dataContext);

                this.hubSpotFormDataSubmitter.SubmitFormData(data, formGuid);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        /// Adds the JSON HubSpot context field to the collection of submitted field data.
        /// </summary>
        /// <param name="data">The key value data containing the name and value for each submitted field of the form.</param>
        /// <param name="dataContext">The data context around the submitted form fields.</param>
        /// <returns>Returns the updated key value data, including the JSON HubSpot context field.</returns>
        protected virtual IDictionary<string, string> AddHubSpotContext(IDictionary<string, string> data, ConnectorFormDataContext dataContext)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (dataContext == null)
            {
                throw new ArgumentNullException("dataContext");
            }

            Dictionary <string, string> hsContext = new Dictionary <string, string> ();
            string hutk = this.GetHutkCookieValue(dataContext.HttpRequest);
            hsContext.Add(HubSpotConnectorFormDataSender.HsContextHutkCookieName, hutk);
            hsContext.Add(HubSpotConnectorFormDataSender.HsContextIpAddress, dataContext.HttpRequest.UserHostAddress);
            hsContext.Add(HubSpotConnectorFormDataSender.HsContextPageUrl, dataContext.SubmitPageUrl.AbsoluteUri);
            hsContext.Add(HubSpotConnectorFormDataSender.HsContextPageName, dataContext.SubmitPageUrl.AbsoluteUri);

            string hsContextJson = JsonConvert.SerializeObject(hsContext);
            if (data.ContainsKey(HubSpotConnectorFormDataSender.HsContextName))
            {
                data[HubSpotConnectorFormDataSender.HsContextName] = hsContextJson;
            }
            else
            {
                data.Add(HubSpotConnectorFormDataSender.HsContextName, hsContextJson);
            }

            return data;
        }

        /// <summary>
        /// Gets the HubSpot hutk cookie from the provided <see cref="HttpRequestBase"/>.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequestBase"/> that the HubSpot hutk cookie value should be extracted from.</param>
        /// <returns>The HubSpot hutk cookie.</returns>
        protected virtual string GetHutkCookieValue(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }

            HttpCookie hutkCookie = httpRequest.Cookies[HubSpotConnectorFormDataSender.HutkCookieName];

            if (hutkCookie == null)
            {
                return null;
            }

            return hutkCookie.Value;
        }

        /// <summary>
        /// Gets the HubSpot form guid from the provided <see cref="ConnectorFormDataContext"/>.
        /// </summary>
        /// <param name="dataContext">The data context around the submitted form fields.</param>
        /// <returns>The HubSpot form guid.</returns>
        protected virtual Guid GetFormGuid(ConnectorFormDataContext dataContext)
        {
            if (dataContext == null)
            {
                throw new ArgumentNullException("dataContext");
            }

            string formFieldName = dataContext.FormDescriptionAttributeSettings[HubSpotFormsConnectorDefinitionsExtender.HubSpotFormNameFieldName];
            HubSpotForm form = this.hubSpotFormsProvider.GetForms().FirstOrDefault(f => f.Name == formFieldName);

            return form.Guid;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed resources
        /// </summary>
        /// <param name="disposing">Defines whether a disposing is executing now.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.hubSpotFormDataSubmitter != null)
                {
                    this.hubSpotFormDataSubmitter.Dispose();
                }
            }
        }

        private readonly IHubSpotFormDataSubmitter hubSpotFormDataSubmitter;
        private readonly IHubSpotFormsProvider hubSpotFormsProvider;

        private const string HutkCookieName = "hubspotutk";
        private const string HsContextHutkCookieName = "hutk";
        private const string HsContextIpAddress = "ipAddress";
        private const string HsContextPageUrl = "pageUrl";
        private const string HsContextPageName = "pageName";
        private const string HsContextName = "hs_context";
    }
}