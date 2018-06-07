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
            var task = new Task { Id = "123", Name = "Тест" };
            var taskService = MockRepository.GenerateStub<ITaskService>();
            taskService
                .Expect(ts => ts.GetTask(Arg<string>.Is.Anything))
                .Return(ActionResult<Task>.Success(task));

            var baseUri = new Uri("http://tfs4alm10v:8080/tfs/TFS2005%20-%20upgraded%20Projects/");

            var idToLinkResult = LinkReplacer.IdToLink("123", taskService, baseUri);
            var result = idToLinkResult.Data;
            Assert.IsTrue(result.Contains("123"));
        }
    }
}
