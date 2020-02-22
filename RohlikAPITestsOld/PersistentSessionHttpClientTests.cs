using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;

namespace RohlikAPITests
{
    [TestClass]
    public class PersistentSessionHttpClientTests
    {
        [TestMethod]
        public void HTTPGet_PersistsCookies()
        {
            const string testCookieName = "testCookieName";
            const string testCookieValue = "testCookieValue";

            var client = new PersistentSessionHttpClient();

            client.Get($"http://httpbin.org/cookies/set/{testCookieName}/{testCookieValue}");
            var response = client.Get("http://httpbin.org/cookies");

            Assert.IsTrue(response.Contains(testCookieName) && response.Contains(testCookieValue), $"Response should contain both {testCookieName} and {testCookieValue}");
        }
    }
}