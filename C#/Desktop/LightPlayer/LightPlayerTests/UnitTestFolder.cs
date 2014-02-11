using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;
using LightPlayer;
using System.Reflection;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestFolder : TestContext
    {
        public override void InitializeParticularDependencies()
        {
            base.InitializeParticularDependencies();

            WindsorContainer.Register(Component.For<IFolder>().ImplementedBy<Folder>());
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

        static public string RealTestPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }
    }
}
