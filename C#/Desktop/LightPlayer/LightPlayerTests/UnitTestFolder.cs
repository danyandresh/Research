using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using System;
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Threading;
using Moq;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;

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

            var frame = new DispatcherFrame();
            folder.Files.CollectionChanged += (sender, e) =>
                {
                    frame.Continue = false;
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(1, e.NewItems.Count);
                    Assert.AreEqual(testFilePath, e.NewItems[0]);
                    synchronizer.Set();
                };

            using (var file = File.Create(testFilePath, 1, FileOptions.DeleteOnClose))
            {
                Dispatcher.PushFrame(frame);
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

            var files = folder.Files;

            Assert.AreEqual(randomFileCount, files.Count, string.Format("There should have been exactly {0} files visible through mask", randomFileCount));
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
            var frame = new DispatcherFrame();
            fileMaskMock.Setup(f => f.IsVisible(It.IsAny<string>())).Returns(() =>
            {
                frame.Continue = false;
                notifyFromMask.Set();
                return true;
            });
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath, fileMask = fileMaskMock.Object });

            folder.OnFileCreated(null, new FileSystemEventArgs(WatcherChangeTypes.Created, expectedPath, "new file popped up.txt"));

            Dispatcher.PushFrame(frame);
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

            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });
            ViewModelLocator.FoldersViewModel.CommandSelectFolder.Execute(folder);

            // Trigger the UI to bind to ObservableCollections (therefore init Files ObservableCollection from STA)
            // and prevent their change from MTA threads
            var binding = new Binding("Files");
            binding.Source = folder;

            var listView = new ListView();
            listView.SetBinding(ListView.ItemsSourceProperty, binding);

            var frame = new DispatcherFrame();

            folder.Files.CollectionChanged += (s, e) =>
            {
                frame.Continue = false;
            };

            var success = false;
            var task = new Task(() =>
                                {
                                    Assert.AreEqual(ApartmentState.MTA, Thread.CurrentThread.GetApartmentState());

                                    var folder1 = ViewModelLocator.PlaylistViewModel.Playlist.Folder;
                                    folder1.OnFileCreated(null, new FileSystemEventArgs(WatcherChangeTypes.Created, expectedPath, "test.mp3"));
                                    success = true;
                                });

            task.Start();

            Dispatcher.PushFrame(frame);

            if (!task.Wait(TimeSpan.FromSeconds(100)))
            {
                Assert.Fail("Task did not finnish in a timely fashion");
            }
            else
            {

                Assert.IsNull(task.Exception/*task.Exception.Flatten().InnerException.ToString()*/);
                Assert.IsTrue(success);
            }
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
