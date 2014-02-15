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
    }
}
