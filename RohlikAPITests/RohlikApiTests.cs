using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;

namespace RohlikAPITests
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