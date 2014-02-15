using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestPlaylistViewModel : TestContext
    {
        [TestMethod]
        public void TestMethodPlaylistVMFailsIfNoSelectedFolderInFolderVM()
        {
            var folderVM = WindsorContainer.Resolve<IFolderViewModel>();
            Assert.IsNull(folderVM.SelectedFolder);

            try
            {
                var vm = WindsorContainer.Resolve<IPlaylistViewModel>();
            }
            catch (DependencyResolverException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Parameter 'toPlay'"), "Missing dependency was not detected");
            }
        }
    }
}
