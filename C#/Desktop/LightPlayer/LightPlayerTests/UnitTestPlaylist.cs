using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LightPlayer;
using System.Collections.ObjectModel;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestPlaylist : TestContext
    {
        [TestMethod]
        public void TestMethodPlaylistLoops()
        {
            var folderMock = new Mock<IFolder>();

            folderMock.Setup(f => f.IsValid).Returns(true);
            folderMock.Setup(f => f.Files).Returns(new ObservableCollection<string>(new[] { string.Empty, "a file" }));
            var playlist = WindsorContainer.Resolve<IPlaylist>(new { folder = folderMock.Object });
            var initialFile = playlist.CurrentFile;
            Assert.IsTrue(playlist.MoveNext());
            Assert.IsTrue(playlist.MoveNext());
            Assert.AreEqual(initialFile, playlist.CurrentFile, "Playlist did not move in the loop");
        }

        [TestMethod]
        public void TestMethodPlaylistIsTransient()
        {
            var folderMock1 = new Mock<IFolder>();
            folderMock1.Setup(f => f.Files).Returns(new ObservableCollection<string>(new[] { string.Empty, "a file" }));
            var playlist1 = WindsorContainer.Resolve<IPlaylist>(new { folder = folderMock1.Object });


            var folderMock2 = new Mock<IFolder>();
            folderMock2.Setup(f => f.Files).Returns(new ObservableCollection<string>(new[] { string.Empty, "a file" }));
            var playlist2 = WindsorContainer.Resolve<IPlaylist>(new { folder = folderMock1.Object });

            Assert.AreNotEqual(playlist1, playlist2, "Playlists should be transient");
        }
    }
}
