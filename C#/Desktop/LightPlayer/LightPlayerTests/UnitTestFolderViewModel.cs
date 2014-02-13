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
        }

        [TestMethod]
        public void TestVMCanStoreModels()
        {
            var mock = new Mock<IFolder>();
            mock.SetupGet(f => f.IsValid).Returns(true);

            var appStateMock = new Mock<IApplicationState>();
            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { appState = appStateMock.Object });
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

            var appStateMock = new Mock<IApplicationState>();
            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { appState = appStateMock.Object });
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
            var appStateMock = new Mock<IApplicationState>();
            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { appState = appStateMock.Object });
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
            var realFolderPath = UnitTestFolder.RealTestPath;

            var folderDialogMock = new Mock<ISelectDialog>();
            folderDialogMock.Setup(f => f.Show()).Returns(new Tuple<DialogResult, string>(DialogResult.Ok, realFolderPath));

            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { selectDialog = folderDialogMock.Object });


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

        [TestMethod]
        public void TestFolderVMCommandAddRealFolder()
        {
            var realFolderPath = UnitTestFolder.RealTestPath;
            var folderDialogMock = new Mock<ISelectDialog>();
            folderDialogMock.Setup(f => f.Show()).Returns(new Tuple<DialogResult, string>(DialogResult.Ok, realFolderPath));

            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { selectDialog = folderDialogMock.Object });

            var command = vm.CommandAddFolder;
            Assert.IsNotNull(command, "Command is null");

            var synchronizer = new ManualResetEvent(false);
            vm.Models.CollectionChanged += (sender, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsTrue(((IFolder)e.NewItems[0]).IsValid);
                Assert.AreEqual(realFolderPath, ((IFolder)e.NewItems[0]).Path);
                synchronizer.Set();
            };

            command.Execute(null);
            if (!synchronizer.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Command has not added a new folder - event handler was not called");
            }
        }

        [TestMethod]
        public void TestFolderVMCommandAddFolderCanBeCanceled()
        {
            var realFolderPath = UnitTestFolder.RealTestPath;
            var folderDialogMock = new Mock<ISelectDialog>();
            folderDialogMock.Setup(f => f.Show()).Returns(new Tuple<DialogResult, string>(DialogResult.Cancel, realFolderPath));

            var vm = WindsorContainer.Resolve<IFolderViewModel>(new { selectDialog = folderDialogMock.Object });

            var command = vm.CommandAddFolder;
            Assert.IsNotNull(command, "Command is null");

            var synchronizer = new ManualResetEvent(false);
            vm.Models.CollectionChanged += (sender, e) =>
            {
                Assert.Fail("When select dialog is canceled do not add anything to the list of folders");
                synchronizer.Set();
            };

            command.Execute(null);

            synchronizer.WaitOne(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void TestFolderVMRemembersFolders()
        {
            var realFolderPath = UnitTestFolder.RealTestPath;

            var vm = TransientFolderVM(WindsorContainer);
            vm.Clear();
            vm.Add(new Folder(realFolderPath));
            WindsorContainer.Release(vm);

            vm = TransientFolderVM(WindsorContainer);

            Assert.AreEqual(1, vm.Models.Count, "FolderVM should store exactly the same number of folders");
            Assert.AreEqual(realFolderPath, vm.Models[0].Path, "FolderVM should store folder paths");
        }

        [TestMethod]
        public void TestFolderVMCanBeDisposed()
        {
            var vm = TransientFolderVM(WindsorContainer);
            WindsorContainer.Release(vm);
            Assert.IsFalse(WindsorContainer.Kernel.ReleasePolicy.HasTrack(vm));

            var currentVm = TransientFolderVM(WindsorContainer);

            Assert.AreNotEqual(vm, currentVm, "FolderVM should have been disposed");
        }

        public static IFolderViewModel TransientFolderVM(IWindsorContainer windsorContainer, dynamic arguments = null)
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IFolderViewModel>()
                .ImplementedBy<FolderViewModel>()
                .DependsOn(Dependency.OnComponent(typeof(ISelectDialog), "folderSelectDialog"))
                .DependsOn(Dependency.OnComponent(typeof(ApplicationState), "applicationState"))
                .LifestyleTransient()
            );

            windsorContainer.AddChildContainer(container);
            var result = container.Resolve<IFolderViewModel>(arguments ?? new { });
            windsorContainer.RemoveChildContainer(container);
            container.Dispose();
            return result;
        }
    }
}
