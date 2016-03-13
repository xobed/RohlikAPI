using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RohlikAPISharp.Tests
{
    [TestClass]
    public class RohlikovacTests
    {
        private string[] GetCredentials()
        {
            var passwordString = File.ReadAllText("loginPassword.txt");
            return passwordString.Split(':');
        }

        [TestMethod]
        public void RunTest()
        {
            var login = GetCredentials();
            var rohlikApi = new RohlikApi(login[0], login[1]);
            var result = rohlikApi.RunRohlikovac();
            if (result.Contains("error"))
            {
                Assert.Fail($"Failed to login to rohlik. Login used: {login[0]}");
            }
        }
    }
}