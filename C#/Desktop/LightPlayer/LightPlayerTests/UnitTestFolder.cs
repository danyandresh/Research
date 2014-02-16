using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using System;
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Threading;
using Moq;
using System.Threading.Tasks;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFolder : TestContext
    {
        public override void InitializeParticularDependencies()
        {
            base.InitializeParticularDependencies();
        }

        [TestMethod]
        public void TestFolderStoresPath()
        {
            string expectedPath = "testFolder";
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.AreEqual(expectedPath, folder.Path, "Path is mandatory for a folder");
        }

        [TestMethod]
        public void TestFolderIsValidIfPathExists()
        {
            string expectedPath = RealTestPath;
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.IsTrue(folder.IsValid, "Folder refers to the executing assembly directory, is should be valid");
        }

        [TestMethod]
        public void TestFolderIsNotValidIfPathDoesntExist()
        {
            string expectedPath = "inexistentFolder";
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.IsFalse(folder.IsValid, "Folder refers to a fictionary location, is should not be valid");
        }

        [TestMethod]
        public void TestFolderListsFiles()
        {
            string expectedPath = RealTestPath;
            var filterMock = new Mock<IFileMask>();
            filterMock.Setup(f => f.IsVisible(It.IsAny<string>())).Returns(true);
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath, fileMask = filterMock.Object });

            var files = Directory.GetFiles(RealTestPath);

            Assert.AreEqual(files.Length, folder.Files.Count(), "Folder does not display correctly the file from directory");
            Assert.IsTrue(files.All(f => folder.Files.Contains(f)), "Not all file from current directory are displayed by Folder");
        }

        [TestMethod]
        public void TestFolderListsNoFilesFromInvalidPath()
        {
            string expectedPath = "";
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.AreEqual(0, folder.Files.Count(), "Folder should display 0 files from invalid path");
        }

        [TestMethod]
        public void TestFolderMonitorsFiles()
        {
            string expectedPath = RealTestPath;
            var fileMaskMock = new Mock<IFileMask>();
            fileMaskMock.Setup(f => f.IsVisible(It.IsAny<string>())).Returns(true);

            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath, fileMask = fileMaskMock.Object });
            var synchronizer = new ManualResetEvent(false);

            var testFilePath = Path.Combine(expectedPath, "new files poped up.txt");

            folder.Files.CollectionChanged += (sender, e) =>
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(1, e.NewItems.Count);
                    Assert.AreEqual(testFilePath, e.NewItems[0]);
                    synchronizer.Set();
                };

            using (var file = File.Create(testFilePath, 1, FileOptions.DeleteOnClose))
            {
                if (!synchronizer.WaitOne(TimeSpan.FromSeconds(1)))
                {
                    Assert.Fail("Monitoring of the files collection in Folder does not work - event handler was not called");
                }
            }
        }

        [TestMethod]
        public void TestFolderSeesOnlyFilesThroughMask()
        {
            string expectedPath = RealTestPath;

            var realFiles = Directory.GetFiles(expectedPath);
            var randomFileCount = new Random().Next(0, realFiles.Length);
            var currentFileCount = 0;

            var filterMock = new Mock<IFileMask>();
            filterMock.Setup(f => f.IsVisible(It.IsAny<string>())).Returns(() =>
            {
                if (currentFileCount >= randomFileCount)
                {
                    return false;
                }

                currentFileCount++;
                return true;
            });

            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath, fileMask = filterMock.Object });

            Assert.AreEqual(randomFileCount, currentFileCount, string.Format("There should have been exactly {0} files visible through mask", randomFileCount));
        }

        [TestMethod]
        public void TestFolderReturnsDifferentInstancesForDifferentPaths()
        {
            var folder1 = WindsorContainer.Resolve<IFolder>(new { path = "c:\\p1" });
            var folder2 = WindsorContainer.Resolve<IFolder>(new { path = "c:\\p2" });

            Assert.AreNotSame(folder1, folder2, "Container should resolve to different folders for different paths");
        }

        [TestMethod]
        public void TestFolderMonitorsFilesUsingMask()
        {
            string expectedPath = RealTestPath;
            var notifyOnNewFile = new ManualResetEvent(false);
            var notifyFromMask = new ManualResetEvent(false);

            var fileMaskMock = new Mock<IFileMask>();
            fileMaskMock.Setup(f => f.IsVisible(It.IsAny<string>())).Returns(() =>
            {
                notifyFromMask.Set();
                return true;
            });
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath, fileMask = fileMaskMock.Object });

            folder.OnFileCreated(null, new FileSystemEventArgs(WatcherChangeTypes.Created, expectedPath, "new file popped up.txt"));

            if (!notifyFromMask.WaitOne(TimeSpan.FromSeconds(1)))
            {
                Assert.Fail("Monitoring of the files collection in Folder while applying mask does not work - most likely mask has not been used");
            }
        }

        [TestMethod]
        public void TestFolderMonitorsFilesCollectionCanBeChangeFromDifferentThread()
        {
            string expectedPath = RealTestPath;

            var uiComplete = new ManualResetEvent(false);

            var appState = WindsorContainer.Resolve<IApplicationState>();
            appState.ClearFolders();
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });
            appState.AddFolder(folder);
            ViewModelLocator.FoldersViewModel.CommandSelectFolder.Execute(folder);

            //Trigger the UI to bind to ObservableCollections and prevent their change from MTA threads
            var app = new App();
            var playlistUI = new LightPlayer.Views.Playlist();

            var success = false;
            var task = new Task(() =>
                                {
                                    Assert.AreEqual(ApartmentState.MTA, Thread.CurrentThread.GetApartmentState());

                                    var folder1 = ViewModelLocator.PlaylistViewModel.Playlist.Folder;
                                    folder1.OnFileCreated(null, new FileSystemEventArgs(WatcherChangeTypes.Created, expectedPath, "test.mp3"));
                                    success = true;
                                });

            task.Start();
            task.Wait();
            Assert.IsNull(task.Exception/*task.Exception.Flatten().InnerException.ToString()*/);
            Assert.IsTrue(success);
        }

        static public string RealTestPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }
    }
}
