using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using LightPlayer;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFoldersViewModel
    {
        protected IWindsorContainer WindsorContainer;

        [TestInitialize]
        public void SetupDependencies()
        {
            WindsorContainer = new WindsorContainer();
            WindsorContainer.Register(Component.For<IFolderViewModel>().ImplementedBy<FolderViewModel>());
        }

        [TestMethod]
        public void TestVMCanStoreModels()
        {
            var mock = new Mock<IFolder>();

            var vm = WindsorContainer.Resolve<IFolderViewModel>();
            vm.Add(mock.Object);
            vm.Add(null);

            Assert.AreEqual(2, vm.Models.Count(),"There should have been two models in the collection");
            Assert.IsTrue(vm.Models.Contains(mock.Object),"Folder could not be added to the FolderViewModel");
        }

    }
}
