﻿using System.IO;
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

        private void CheckOrderHistoryItem(Order order)
        {
            Assert.IsTrue(order.Id > 0);
            Assert.IsTrue(order.Price > 0);
            Assert.IsNotNull(order.Date);
            Assert.IsTrue(!string.IsNullOrEmpty(order.PaymentMethod));
        }

        [TestMethod, TestCategory("Authenticated")]
        public void OrderHistory_RetrievesHistory()
        {
            var login = GetCredentials();
            var rohlikApi = new AuthenticatedRohlikApi(login[0], login[1]);
            var result = rohlikApi.GetOrderHistory().ToList();
            Assert.IsTrue(result.Any(), "Failed to get any orders");

            result.ForEach(CheckOrderHistoryItem);
        }

        [TestMethod, TestCategory("Authenticated")]
        public void RunTest()
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