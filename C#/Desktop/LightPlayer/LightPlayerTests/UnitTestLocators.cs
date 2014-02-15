using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;


namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestLocators : TestContext
    {
        [TestMethod]
        public void TestLocatorProvidesFoldersVM()
        {
            Assert.IsNotNull(ViewModelLocator.FoldersViewModel, "ViewModelLocator does not create folders VM");
        }

        [TestMethod]
        public void TestDependencyContainerSetup()
        {
            Assert.IsNotNull(ViewModelLocator.DependecyContainer);
        }

        [TestMethod]
        public void TestDependencyContainerSingleton()
        {
            Assert.AreEqual(ViewModelLocator.DependecyContainer, ViewModelLocator.DependecyContainer, "Dependency container should obtain the same instance always");
        }

        [TestMethod]
        public void TestSetupDependencyContainerReturnsContainer()
        {
            Assert.IsNotNull(ViewModelLocator.SetupDependencyContainer());
        }

        [TestMethod]
        public void TestLocatorResolvesFolderViewModelFromContainer()
        {
            var vm = ViewModelLocator.DependecyContainer.Resolve<IFolderViewModel>();

            Assert.AreEqual(vm, ViewModelLocator.FoldersViewModel);
        }

        [TestMethod]
        public void TestLocatorPlaylistVMIsTransient()
        {
            var folderVM = ViewModelLocator.FoldersViewModel;
            var testFolder = WindsorContainer.Resolve<IFolder>(new { path = UnitTestFolder.RealTestPath });
            folderVM.CommandSelectFolder.Execute(testFolder);

            Assert.IsNotNull(folderVM.SelectedFolder);

            var playlist1 = ViewModelLocator.PlaylistViewModel;
            var playlist2 = ViewModelLocator.PlaylistViewModel;

            Assert.AreNotSame(playlist1, playlist2);
        }

    }
}
