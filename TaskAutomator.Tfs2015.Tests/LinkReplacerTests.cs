using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskAutomator.Tfs2015.Tests.Data;

namespace TaskAutomator.Tfs2015.Tests
{
    [TestClass]
    public class LinkReplacerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            new LinkReplacer().Process("").Equals(null);
            TestData.Before_1.Equals(null);
        }
    }
}
