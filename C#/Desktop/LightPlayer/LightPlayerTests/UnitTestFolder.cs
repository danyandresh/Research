using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;
using LightPlayer;
using System.Reflection;
using System;
using System.IO;

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
            string expectedPath = AssemblyDirectory;
            var folder = WindsorContainer.Resolve<IFolder>(new { path = expectedPath });

            Assert.IsTrue(folder.IsValid, "Folder refers to the executing assembly directory, is should be valid");
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
