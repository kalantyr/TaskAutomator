using System;
using NUnit.Framework;
using Rhino.Mocks;
using TaskAutomator.Core;
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

        [Test]
        public void IdToLinkTest()
        {
            var baseUri = new Uri("http://tfs4alm10v:8080/tfs/TFS2005%20-%20upgraded%20Projects/");
            var taskService = MockRepository.GenerateStub<ITaskService>();
            var result = LinkReplacer.IdToLink("140263", taskService, baseUri);
            Assert.IsNotNull(result);
        }
    }
}
