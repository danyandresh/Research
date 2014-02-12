using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using LightPlayer;
using System.Collections.Specialized;
using System.Threading;
using System;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFoldersViewModel : TestContext
    {
        public override void InitializeParticularDependencies()
        {
            base.InitializeParticularDependencies();

            WindsorContainer.Register(Component.For<IFolderViewModel>().ImplementedBy<FolderViewModel>());
        }

        [TestMethod]
        public void TestVMCanStoreModels()
        {
            var mock = new Mock<IFolder>();
            mock.SetupGet(f => f.IsValid).Returns(true);

            var vm = WindsorContainer.Resolve<IFolderViewModel>();
            vm.Add(mock.Object);
            vm.Add(null);

            Assert.AreEqual(2, vm.Models.Count(), "There should have been two models in the collection");
            Assert.IsTrue(vm.Models.Contains(mock.Object), "Folder could not be added to the FolderViewModel");
        }

        [TestMethod]
        public void TestVMStoresOnlyValidModels()
        {
            var mockValidModel = new Mock<IFolder>();
            mockValidModel.SetupGet(f => f.IsValid).Returns(true);

            var mockInvalidModel = new Mock<IFolder>();
            mockInvalidModel.SetupGet(f => f.IsValid).Returns(false);

            var vm = WindsorContainer.Resolve<IFolderViewModel>();
            vm.Add(mockValidModel.Object);
            vm.Add(mockInvalidModel.Object);

            Assert.AreEqual(1, vm.Models.Count(), "There should have been a single model in the collection");
            Assert.IsTrue(vm.Models.Contains(mockValidModel.Object), "Valid model was not be added to the FolderViewModel");
        }

        [TestMethod]
        public void TestVMMonitorsFolders()
        {
            var mockValidModel = new Mock<IFolder>();
            mockValidModel.SetupGet(f => f.IsValid).Returns(true);

            var synchronizer = new ManualResetEvent(false);
            var vm = WindsorContainer.Resolve<IFolderViewModel>();
            vm.Models.CollectionChanged += (sender, e) =>
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(1, e.NewItems.Count);
                    Assert.AreEqual(mockValidModel.Object, e.NewItems[0]);
                    synchronizer.Set();
                };

            vm.Add(mockValidModel.Object);

            if (!synchronizer.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Monitoring of the folders collection does not work - event handler was not called");
            }
        }

        [TestMethod]
        public void TestFolderVMCommandAddFolder()
        {
            var vm = WindsorContainer.Resolve<IFolderViewModel>();

            var command = vm.CommandAddFolder;
            Assert.IsNotNull(command, "Command is null");

            var synchronizer = new ManualResetEvent(false);
            vm.Models.CollectionChanged += (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsTrue(((IFolder)e.NewItems[0]).IsValid);
                synchronizer.Set();
            };

            command.Execute(null);
            if (!synchronizer.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Command has not added a new folder - event handler was not called");
            }
        }
    }
}
