using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using System.IO;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFileMask : TestContext
    {
        [TestMethod]
        public void TestFileMaskSeesMp3()
        {
            var fileMask = WindsorContainer.Resolve<IFileMask>();
            var fileName = "file.mp3";

            Assert.IsTrue(fileMask.IsVisible(fileName));
        }

        [TestMethod]
        public void TestFileMaskDoesNotSeeMp4()
        {
            var fileMask = WindsorContainer.Resolve<IFileMask>();
            var fileName = "file.mp4";

            Assert.IsFalse(fileMask.IsVisible(fileName));
        }
    }
}
