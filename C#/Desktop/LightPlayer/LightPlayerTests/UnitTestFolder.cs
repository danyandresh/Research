using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;
using LightPlayer;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFolder : TestContext
    {
        public override void InitializeParticularDependencies()
        {
            base.InitializeParticularDependencies();

            WindsorContainer.Register(Component.For<IFolder>().ImplementedBy<Folder>());
        }

        [TestMethod]
        public void TestFolderStoresPath()
        {
            string expectedPath = "testFolder";
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.AreEqual(expectedPath, folder.Path, "Path is mandatory for a folder");
        }
    }
}
