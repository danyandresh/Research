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
            var fileInfo=new FileInfo("file.mp3");

            Assert.IsTrue(fileMask.IsVisible(fileInfo.Name));
        }
    }
}
