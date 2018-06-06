using NUnit.Framework;
using TaskAutomator.Tfs2015.Tests.Data;

namespace TaskAutomator.Tfs2015.Tests
{
    [TestFixture]
    public class LinkReplacerTests
    {
        [Test]
        public void TestMethod1()
        {
            var result = LinkReplacer.ReplaceLinks(TestData.Before_1);
            Assert.AreEqual(TestData.After_1, result);
        }
    }
}
