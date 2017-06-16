using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Configuration;
using Telerik.Sitefinity.HubSpotConnector.Model;
using Telerik.Sitefinity.Test.Unit.HubSpot.HubSpotFormsClientTests.Mocks;

namespace Telerik.Sitefinity.Test.Unit.HubSpot.HubSpotFormsClientTests
{
    /// <summary>
    /// HubSpot form client GetForms unit tests.
    /// </summary>
    [TestClass]
    public class HubSpotFormClient_GetForms_Should
    {
        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns server error.
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

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            client.GetForms();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns unauthorized.
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

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            client.GetForms();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns forbidden.
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

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            client.GetForms();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns bad request.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsBadRequest()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            };

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            client.GetForms();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsNotFound()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            };

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            client.GetForms();
        }

        /// <summary>
        /// Tests whether the method will return empty list when service returns empty list of elements.
        /// </summary>
        [TestMethod]
        public void ReturnEmptyList_WhenServiceReturnsEmptyListOfElements()
        {
            // Arrange
            string responseContent = "[]";

            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponseMessage.Content = new StringContent(responseContent);

                return httpResponseMessage;
            };

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            IEnumerable<HubSpotForm> result = client.GetForms();

            // Assert
            Assert.IsFalse(result.Any());
        }

        /// <summary>
        /// Tests whether the method will return correct list of elements when service returns list of elements.
        /// </summary>
        [TestMethod]
        public void ReturnCorrectListOfElements_WhenServiceReturnsListOfElements()
        {
            // Arrange
            IList<HubSpotForm> expectedResult = HubSpotFormModelMocksProvider.CreateMockedFormsCollection(2, 2, 2).ToList();

            string responseContent = new JavaScriptSerializer().Serialize(expectedResult);

            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponseMessage.Content = new StringContent(responseContent);

                return httpResponseMessage;
            };

            // Act
            HubSpotFormsClient client = new HubSpotFormsClient(new HubSpotConnectorConfig(), new HttpClient(httpMessageHandlerMock), new FormUrlBuilder());
            IList<HubSpotForm> actualResult = client.GetForms().ToList();

            // Assert
            Assert.IsTrue(actualResult.Any());
            Assert.AreEqual(expectedResult.Count(), actualResult.Count());

            for (int i = 0; i < expectedResult.Count(); i++)
            {
                Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedResult[i], actualResult[i]));
            }
        }
    }
}