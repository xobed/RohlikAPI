using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPITests
{
    [TestClass]
    public class AuthenticatedRohlikApiTests
    {
        [TestMethod]
        public void RohlikApiLogin_FailsToLoginWithWrongPassword()
        {
            try
            {
                var authenticatedApiClient = new AuthenticatedRohlikApi("testemail@testemail.com", "TestPassword");
                authenticatedApiClient.GetOrderHistory();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Failed to login"),
                    $"Unexpected exception message: {ex.Message}");
                return;
            }

            Assert.Fail("Login with fake credentials did not throw an exception");
        }

        private string[] GetCredentials()
        {
            var credentialsString = Environment.GetEnvironmentVariable("TEST_CREDENTIALS");

            if (credentialsString == null)
            {
                throw new ArgumentException(
                    "Test needs credentials. Set environment variable 'TEST_CREDENTIALS=username@email.com:yourPassword'");
            }

            return credentialsString.Split(':');
        }

        private void CheckOrderHistoryItem(Order order)
        {
            Assert.IsTrue(order.Id > 0);
            Assert.IsNotNull(order.Date);
        }

        [TestMethod, TestCategory("Authenticated")]
        public void OrderHistory_RetrievesAllHistory()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);
            var result = rohlikApi.GetOrderHistory().ToList();
            Assert.IsTrue(result.Any(), "Failed to get any orders");

            result.ForEach(CheckOrderHistoryItem);
        }


        [TestMethod, TestCategory("Authenticated")]
        public void RunRohlikovac()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);
            var result = rohlikApi.RunRohlikovac();

            Assert.IsTrue(result.Status == 200);
        }
    }
}