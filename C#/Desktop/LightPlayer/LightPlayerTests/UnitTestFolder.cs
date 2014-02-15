using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using System;
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Threading;

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
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

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
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });
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
        public void TestFolderReturnsDifferentInstancesForDifferentPaths()
        {
            var folder1 = WindsorContainer.Resolve<IFolder>(new { path = "c:\\p1" });
            var folder2 = WindsorContainer.Resolve<IFolder>(new { path = "c:\\p2" });

            Assert.AreNotSame(folder1, folder2, "Container should resolve to different folders for different paths");
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
