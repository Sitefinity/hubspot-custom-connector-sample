using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.BackgroundTasks;
using Telerik.Sitefinity.HubSpotConnector;
using Telerik.Sitefinity.HubSpotConnector.Client.Forms;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.HubSpot.HubSpotFormsCacheTests
{
    /// <summary>
    /// HubSpot form client HubSpotFormsCache GetForms unit tests.
    /// </summary>
    [TestClass]
    public class HubSpotFormsCache_GetForms_Should
    {
        /// <summary>
        /// Initializes the logic used before each of the tests in the test class run.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.hubSpotFormsClient = Mock.Create<IHubSpotFormsClient>();
            this.cacheManager = this.CreateCacheManagerMock();
        }

        /// <summary>
        /// Tests whether the method will return a collection when calling the method for the first time.
        /// </summary>
        [TestMethod]
        public void ReturnCollection_WhenCallingGetForTheFirstTime()
        {
            // Arrange
            IEnumerable<HubSpotForm> expectedForms = HubSpotFormModelMocksProvider.CreateMockedFormsCollection(5, 0, 0);
            Mock.Arrange(() => this.hubSpotFormsClient.GetForms()).Returns(expectedForms);

            IHubSpotFormsCache hubSpotFormsCache = new HubSpotFormsCache(this.hubSpotFormsClient, this.cacheManager);
            
            // Act
            IEnumerable<HubSpotForm> actualForms = hubSpotFormsCache.GetForms();

            // Assert
            Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedForms, actualForms));
        }

        /// <summary>
        /// Tests whether the method will return an empty collection when the original service call returns an empty collection.
        /// </summary>
        [TestMethod]
        public void ReturnEmptyCollection_WhenOriginalCollectionIsEmpty()
        {
            // Arrange
            IEnumerable<HubSpotForm> expectedForms = Enumerable.Empty<HubSpotForm>();
            Mock.Arrange(() => this.hubSpotFormsClient.GetForms()).Returns(expectedForms);

            IHubSpotFormsCache hubSpotFormsCache = new HubSpotFormsCache(this.hubSpotFormsClient, this.cacheManager);
            
            // Act
            IEnumerable<HubSpotForm> actualForms = hubSpotFormsCache.GetForms();

            // Assert
            Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedForms, actualForms));
        }

        /// <summary>
        /// Tests whether the method will return a cached collection after the first get.
        /// </summary>
        [TestMethod]
        public void ReturnCachedCollection_WhenCallingGetAfterFirstTime()
        {
            // Arrange
            IEnumerable<HubSpotForm> expectedForms = HubSpotFormModelMocksProvider.CreateMockedFormsCollection(5, 0, 0);
            Mock.Arrange(() => this.hubSpotFormsClient.GetForms()).Returns(expectedForms);

            IHubSpotFormsCache hubSpotFormsCache = new HubSpotFormsCache(this.hubSpotFormsClient, this.cacheManager);

            // Act
            hubSpotFormsCache.GetForms();
            Mock.Arrange(() => this.hubSpotFormsClient.GetForms())
                .Returns(HubSpotFormModelMocksProvider.CreateMockedFormsCollection(3, 0, 0));
            IEnumerable<HubSpotForm> actualForms = hubSpotFormsCache.GetForms();

            // Assert
            Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedForms, actualForms));
        }

        /// <summary>
        /// Tests whether the method will update cached collection after cache expires.
        /// </summary>
        [TestMethod]
        public void UpdateCachedCollection_WhenCacheExpires()
        {
            // Arrange
            IEnumerable<HubSpotForm> expectedInitialForms = HubSpotFormModelMocksProvider.CreateMockedFormsCollection(5, 0, 0);
            Mock.Arrange(() => this.hubSpotFormsClient.GetForms()).Returns(expectedInitialForms);

            var backgroundTaskService = Mock.Create<IBackgroundTasksService>();
            Mock.Arrange(() => backgroundTaskService.EnqueueTask(Arg.IsAny<Action>())).DoInstead((Action action) => { action(); });

            IHubSpotFormsCache hubSpotFormsCache = new HubSpotFormsCache(this.hubSpotFormsClient, this.cacheManager);

            ObjectFactory.RunWithContainer(
                new UnityContainer(),
                () =>
                {
                    // Act
                    ObjectFactory.Container.RegisterInstance<IBackgroundTasksService>(backgroundTaskService);
                    IEnumerable<HubSpotForm> actualInitialForms = hubSpotFormsCache.GetForms();

                    this.SetCacheItemsAsExpired();

                    IEnumerable<HubSpotForm> expectedUpdatedForms = HubSpotFormModelMocksProvider.CreateMockedFormsCollection(5, 0, 0);
                    Mock.Arrange(() => this.hubSpotFormsClient.GetForms()).Returns(expectedUpdatedForms);

                    IEnumerable<HubSpotForm> actualExpiredForms = hubSpotFormsCache.GetForms();
                    IEnumerable<HubSpotForm> actualUpdatedForms = hubSpotFormsCache.GetForms();

                    // Assert
                    Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedInitialForms, actualInitialForms));
                    Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedInitialForms, actualExpiredForms));
                    Assert.IsTrue(HubSpotFormModelsComparer.AreEqual(expectedUpdatedForms, actualUpdatedForms));
                });
        }

        private ICacheManager CreateCacheManagerMock()
        {
            ICacheManager cacheManager = Mock.Create<ICacheManager>();

            this.cachedObjects = new Dictionary<string, object>();
            Mock.Arrange(() => cacheManager.Add(Arg.AnyString, Arg.AnyObject)).DoInstead((string key, object obj) => { cachedObjects[key] = obj; });
            Mock.Arrange(() => cacheManager[Arg.AnyString]).Returns((string key) =>
            { 
                if (cachedObjects.ContainsKey(key)) 
                { 
                    return cachedObjects[key]; 
                }

                return null;
            });

            return cacheManager;
        }

        private void SetCacheItemsAsExpired()
        {
            if (this.cachedObjects != null && !this.cachedObjects.Any())
            {
                return;
            }

            foreach (KeyValuePair<string, object> item in this.cachedObjects)
            {
                (item.Value as CachedItemWrapper<IEnumerable<HubSpotForm>>).AddedToCacheTimeStamp = DateTime.MinValue;
            }
        }

        private IDictionary<string, object> cachedObjects;
        private IHubSpotFormsClient hubSpotFormsClient;
        private ICacheManager cacheManager;
    }
}