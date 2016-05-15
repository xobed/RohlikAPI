﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPISharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPISharp.Tests
{
    [TestClass()]
    public class RohlikApiTests
    {
        [TestMethod()]
        public void RohlikApiTest()
        {
            Assert.IsNotNull(new RohlikApi(City.Brno));
            Assert.IsNotNull(new RohlikApi(City.Praha));
            
        }
    }
}