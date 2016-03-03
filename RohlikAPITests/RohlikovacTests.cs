using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPISharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPISharp.Tests
{
    [TestClass()]
    public class RohlikovacTests
    {
        private string[] GetCredentials()
        {
            var passwordString = File.ReadAllText("loginPassword.txt");
            return passwordString.Split(':');
        }

        [TestMethod()]
        public void RunTest()
        {
            var login = GetCredentials();
            var rohlikApi = new RohlikApi(login[0], login[1]);
            string result = rohlikApi.RunRohlikovac();
            if (result.Contains("error"))
            {
                Assert.Fail($"Failed to login to rohlik. Login used: {login[0]}");
            }
        }
    }
}