using System;
using System.Collections.Specialized;

namespace Telerik.Sitefinity.HubSpotConnector.Client.Forms
{
    /// <inheritdoc/>
    internal class FormUrlBuilder : IFormUrlBuilder
    {
        /// <inheritdoc/>
        public string BuildFormsUrl(string apiKey = null)
        {
            string path = string.Format("/{0}/{1}/{2}", FormUrlBuilder.FormPlural, FormUrlBuilder.ApiVersion, FormUrlBuilder.FormPlural);
            string query = this.BuildUrlQuery(apiKey);
            string pathAndQuery = string.Concat(path, query).ToLower();

            return pathAndQuery;
        }

        /// <inheritdoc/>
        public string BuildSubmitFormUrl(string portalId, Guid formGuid)
        {
            if (string.IsNullOrWhiteSpace(portalId))
            {
                throw new ArgumentException("portalId cannot be null, empty or whitespace");
            }

            string path = string.Format("/{0}/{1}/{2}/{3}/{4}", FormUrlBuilder.UploadPlural, FormUrlBuilder.FormSingular, FormUrlBuilder.ApiVersion, portalId, formGuid);

            return path;
        }

        /// <summary>
        /// Builds a URL query that is used to call the REST API based on the provided parameters.
        /// </summary>
        /// <param name="apiKey">The depth of the form response.</param>
        /// <returns>Returns the URL query.</returns>
        protected virtual string BuildUrlQuery(string apiKey = null)
        {
            NameValueCollection queryCollection = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                queryCollection.Add(FormUrlBuilder.ApiKeyQueryStringParamKey, apiKey);
            }

            string queryString = string.Empty;
            if (queryCollection.Count > 0)
            {
                queryString = queryCollection.ToQueryString();
            }

            return queryString;
        }

        private const string ApiVersion = "v2";
        private const string FormSingular = "form";
        private const string FormPlural = "forms";
        private const string UploadPlural = "uploads";
        private const string ApiKeyQueryStringParamKey = "hapikey";
    }
}