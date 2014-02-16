using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers;
using Moq;
using System.Threading;
using System;
using System.Collections.ObjectModel;

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
                Assert.IsTrue(ex.Message.Contains("Parameter 'folder'"), "Missing dependency was not detected");
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

            Assert.AreSame(folderVM.SelectedFolder, playlistVM.Playlist.Folder);
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

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>(new[] { playFile }));

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            var manualResetEvent = new ManualResetEvent(false);
            playlistVM.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("CurrentlyPlaying", e.PropertyName);
                Assert.AreEqual(playFile, ((IPlaylistViewModel)s).CurrentlyPlaying);
                manualResetEvent.Set();
            };

            playlistVM.CurrentlyPlaying = playFile;

            if (!manualResetEvent.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("CurrentlyPlaying file is not observed");
            }
        }

        [TestMethod]
        public void TestMethodPlaylistVMStartsWithFirstFile()
        {
            var testFolder = new Mock<IFolder>();
            var playFile = string.Empty;

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>(new[] { playFile, null }));

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            Assert.AreEqual(playFile, playlistVM.CurrentlyPlaying);
        }

        [TestMethod]
        public void TestMethodPlaylistVMStartsWithNoFileIfEmptyFolder()
        {
            var testFolder = new Mock<IFolder>();

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>());

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            Assert.IsNull(playlistVM.CurrentlyPlaying);
        }

        [TestMethod]
        public void TestMethodPlaylistVMPlayCommandStopsAndStartsWithNewFile()
        {
            var testFolder = new Mock<IFolder>();
            var playFile = string.Empty;

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>(new[] { playFile, null }));

            var mediaElement = new Mock<IMediaElement>();

            EventWaitHandle stopEH = new ManualResetEvent(false),
                changeEH = new ManualResetEvent(false),
                startEH = new ManualResetEvent(false);
            mediaElement.Setup(me => me.Stop).Returns(() => { stopEH.Set(); });
            mediaElement.Setup(me => me.Play).Returns(() =>
            {
                // make sure source is being changed before playing
                Assert.IsTrue(changeEH.WaitOne(TimeSpan.FromSeconds(1)), "Source was never changed");
                startEH.Set();
            });

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            playFile = "new file";
            playlistVM.PropertyChanged += (s, e) =>
            {
                // make sure Stop is called in the first instance
                Assert.IsTrue(stopEH.WaitOne(TimeSpan.FromSeconds(1)), "Stop was never called");
                Assert.AreEqual("CurrentlyPlaying", e.PropertyName);
                Assert.AreEqual(playFile, ((IPlaylistViewModel)s).CurrentlyPlaying);
                changeEH.Set();
            };

            playlistVM.CommandPlayFile.Execute(new Tuple<IMediaElement, string>(mediaElement.Object, playFile));

            if (!startEH.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Start was never called");
            }
        }

        [TestMethod]
        public void TestMethodPlaylistVMPlayCommandStarts()
        {
            var testFolder = new Mock<IFolder>();
            var playFile = string.Empty;

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>(new[] { playFile, null }));

            var mediaElement = new Mock<IMediaElement>();

            EventWaitHandle changeEH = new ManualResetEvent(false),
            startEH = new ManualResetEvent(false);
            mediaElement.Setup(me => me.Play).Returns(() =>
            {
                startEH.Set();
            });

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            playFile = "new file";
            playlistVM.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("CurrentlyPlaying", e.PropertyName);
                Assert.AreEqual(playFile, ((IPlaylistViewModel)s).CurrentlyPlaying);
                changeEH.Set();
            };

            playlistVM.CommandPlay.Execute(mediaElement.Object);

            if (!startEH.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Start was never called");
            }
        }

        [TestMethod]
        public void TestMethodPlaylistVMStopCommandStops()
        {
            var testFolder = new Mock<IFolder>();
            var playFile = string.Empty;

            testFolder.Setup(t => t.Files).Returns(new ObservableCollection<string>(new[] { playFile, null }));

            var mediaElement = new Mock<IMediaElement>();

            var stopEH = new ManualResetEvent(false);
            mediaElement.Setup(me => me.Stop).Returns(() =>
            {
                stopEH.Set();
            });

            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = testFolder.Object });

            var playlistVM = WindsorContainer.Resolve<IPlaylistViewModel>(new { playlist = playlist });

            playlistVM.CommandStop.Execute(mediaElement.Object);

            if (!stopEH.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Stop was never called");
            }
        }
    }
}
