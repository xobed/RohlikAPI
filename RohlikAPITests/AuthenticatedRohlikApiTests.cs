using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;

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
    }
}