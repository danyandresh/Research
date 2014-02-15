using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers;
using Moq;
using System.Threading;
using System;

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

        [TestMethod]
        public void TestMethodPlaylistVMBuildsOnSelectedFolderInFolderVM()
        {
            var folderVM = WindsorContainer.Resolve<IFolderViewModel>();
            var testFolder = WindsorContainer.Resolve<IFolder>(new { path = UnitTestFolder.RealTestPath });
            folderVM.CommandSelectFolder.Execute(testFolder);
            Assert.IsNotNull(folderVM.SelectedFolder);

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>();

            Assert.AreSame(folderVM.SelectedFolder, playlistVM.Folder);
        }

        [TestMethod]
        public void TestMethodPlaylistVMIsTransient()
        {
            var folderVM = WindsorContainer.Resolve<IFolderViewModel>();
            var testFolder = WindsorContainer.Resolve<IFolder>(new { path = UnitTestFolder.RealTestPath });
            folderVM.CommandSelectFolder.Execute(testFolder);

            Assert.IsNotNull(folderVM.SelectedFolder);

            var playlist1 = WindsorContainer.Resolve<IPlaylistViewModel>();
            var playlist2 = WindsorContainer.Resolve<IPlaylistViewModel>();

            Assert.AreNotSame(playlist1, playlist2);
        }


        [TestMethod]
        public void TestMethodPlaylistVMObservesCurrentlyPlaying()
        {
            var testFolder = new Mock<IFolder>();
            var playFile = string.Empty;

            var playlist = WindsorContainer.Resolve<IPlaylistViewModel>(new { toPlay = testFolder.Object });

            var manualResetEvent = new ManualResetEvent(false);
            playlist.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("CurrentlyPlaying", e.PropertyName);
                Assert.AreEqual(playFile, ((IPlaylistViewModel)s).CurrentlyPlaying);
                manualResetEvent.Set();
            };

            playlist.CurrentlyPlaying = playFile;

            if (!manualResetEvent.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("CurrentlyPlaying file is not observed");
            }
        }
    }
}
