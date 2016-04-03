﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RohlikAPISharp.Tests
{
    [TestClass]
    public class RohlikovacTests
    {
        private string[] GetCredentials()
        {
            string filePath = @"..\..\..\loginPassword.txt";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Test needs credentials. Create 'loginPassword.txt' in solution root directory. Enter username@email.com:yourPassword on first line.");
            }
            var passwordString = File.ReadAllText(filePath);
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