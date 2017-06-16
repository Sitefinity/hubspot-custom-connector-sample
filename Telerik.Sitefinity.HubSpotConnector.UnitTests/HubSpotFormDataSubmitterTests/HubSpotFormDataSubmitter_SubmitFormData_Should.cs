using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.Test.Unit.HubSpot.HubSpotFormsClientTests.Mocks;

namespace Telerik.Sitefinity.Test.Unit.HubSpot.HubSpotFormDataSubmitterTests
{
    /// <summary>
    /// HubSpot form data submitter SubmitFormDataAsync unit tests.
    /// </summary>
    [TestClass]
    public class HubSpotFormDataSubmitter_SubmitFormData_Should
    {
        /// <summary>
        /// Initializes the logic used before each of the tests in the test class run.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.hubSpotConnectorConfig = new HubSpotConnectorConfig();
            this.hubSpotConnectorConfig.HubSpotPortalId = "portalId";
            this.hubSpotConnectorConfig.HubSpotApiKey = "apikey";
        }

        /// <summary>
        /// Tests whether the method will throw ArgumentNullException exception
        /// when the name value collection containing the form fields is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowException_WhenNameValueCollectionIsNull()
        {
            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter = new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(null, Guid.NewGuid());
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException exception
        /// when the server returns server error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsServerError()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) =>
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            };

            var formData = new Dictionary<string, string>()
            {
                { "test", "test" }
            };

            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter =
                new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(formData, Guid.NewGuid());
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException exception
        /// when the server returns bad request.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsBadREquest()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) =>
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            };

            var formData = new Dictionary<string, string>()
            {
                { "test", "test" }
            };

            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter =
                new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(formData, Guid.NewGuid());
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException exception
        /// when the server returns forbidden.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsForbidden()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) =>
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            };

            var formData = new Dictionary<string, string>()
            {
                { "test", "test" }
            };

            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter =
                new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(formData, Guid.NewGuid());
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException exception
        /// when the server returns unauthorized.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsUnauthorized()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) =>
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            };

            var formData = new Dictionary<string, string>()
            {
                { "test", "test" }
            };

            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter =
                new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(formData, Guid.NewGuid());
        }

        /// <summary>
        /// Tests whether the correct request body is created.
        /// </summary>
        [TestMethod]
        public void CreatesCorrectRequestData_WhenCorrectDataSet()
        {
            // Arrange
            string requestContent = string.Empty;
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) =>
            {
                requestContent = httpRequestMessage.Content.ReadAsStringAsync().Result;
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                responseMessage.Content = new StringContent(string.Empty);

                return responseMessage;
            };

            var formData = new Dictionary<string, string>()
            {
                { "test", "test" }
            };

            // Act
            IHubSpotFormDataSubmitter hubSpotFormDataSubmitter =
                new HubSpotFormDataSubmitter(this.hubSpotConnectorConfig, new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            hubSpotFormDataSubmitter.SubmitFormData(formData, Guid.NewGuid());

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(requestContent));

            var responseDictionary = requestContent.Split('&').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);
            Assert.AreEqual(responseDictionary["test"], "test");
        }

        private HubSpotConnectorConfig hubSpotConnectorConfig;
    }
}