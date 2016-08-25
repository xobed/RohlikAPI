using System;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;
using RohlikAPI.Model;

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
                Assert.IsTrue(ex.Message.Contains("Failed to login to Rohlik"), $"Unexpected exception message: {ex.Message}");
                return;
            }
            Assert.Fail("Login with fake credentials did not throw an exception");
        }

        private string[] GetCredentials()
        {
            var filePath = @"..\..\..\loginPassword.txt";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Test needs credentials. Create 'loginPassword.txt' in solution root directory. Enter username@email.com:yourPassword on first line.");
            }
            var passwordString = File.ReadAllText(filePath);
            return passwordString.Split(':');
        }

        private void CheckArticle(Article article)
        {
            Assert.IsFalse(string.IsNullOrEmpty(article.Name));
            Assert.IsNotNull(article.Price);
            Assert.IsNotNull(article.Count);
        }

        private void CheckOrderHistoryItem(Order order, DateTime timeSince)
        {
            Assert.IsTrue(order.Id > 0);
            Assert.IsTrue(order.Price > 0);
            Assert.IsNotNull(order.Date);
            Assert.IsTrue(!string.IsNullOrEmpty(order.PaymentMethod));
            Assert.IsTrue(order.Date > timeSince);
            Assert.IsTrue(order.Articles.Any(), "Failed to get any order articles");
            order.Articles.ToList().ForEach(CheckArticle);
        }

        [TestMethod, TestCategory("Authenticated")]
        public void OrderHistory_RetrievesAllHistory()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);
            var result = rohlikApi.GetOrderHistory().ToList();
            Assert.IsTrue(result.Any(), "Failed to get any orders");

            result.ForEach(order => CheckOrderHistoryItem(order, DateTime.MinValue));
        }

        [TestMethod, TestCategory("Authenticated")]
        public void OrderHistory_RetrievesHistorySinceDate()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);

            var timeBeforeLast3Months = DateTime.Now.AddMonths(-1);
            var result = rohlikApi.GetOrderHistory(timeBeforeLast3Months).ToList();
            Assert.IsTrue(result.Any(), "Failed to get any orders");

            result.ForEach(order => CheckOrderHistoryItem(order, timeBeforeLast3Months));
        }

        [TestMethod, TestCategory("Authenticated")]
        public void RunRohlikovac()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);
            var result = rohlikApi.RunRohlikovac();
            if (result.Contains("error"))
            {
                Assert.Fail($"Failed to login to rohlik. Login used: {login[0]}");
            }
        }
    }
}