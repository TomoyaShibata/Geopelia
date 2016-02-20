using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace GeopeliaTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var actual = 10 + 10;
            Assert.AreEqual(20, actual);
        }
    }
}
