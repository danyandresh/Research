using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestApplicationState : TestContext
    {
        [TestMethod]
        public void TestApplicationStateStoresModelUniquely()
        {
            var testFolder = "whatever folder name";
            var appState = TransientApplicationState(WindsorContainer);
            appState.ClearFolders();
            appState.AddFolder(WindsorContainer.Resolve<IFolder>(new { path = testFolder }));
            appState.AddFolder(WindsorContainer.Resolve<IFolder>(new { path = testFolder }));

            Assert.AreEqual(1, appState.Folders.Count());
            Assert.AreEqual(testFolder, appState.Folders.First().Path);
        }

        [TestMethod]
        public void TestApplicationStateStoresModel()
        {
            var testFolder = "whatever folder name";
            var appState = TransientApplicationState(WindsorContainer);
            appState.ClearFolders();
            appState.AddFolder(WindsorContainer.Resolve<IFolder>(new { path = testFolder }));

            WindsorContainer.Release(appState);
            var appStateCurrent = TransientApplicationState(WindsorContainer);

            Assert.AreEqual(1, appStateCurrent.Folders.Count());
            Assert.AreEqual(testFolder, appStateCurrent.Folders.First().Path);
        }

        [TestMethod]
        public void TestApplicationStateRestoredModelRemainsValid()
        {
            var testFolderPath = UnitTestFolder.RealTestPath;
            var appState = TransientApplicationState(WindsorContainer);
            appState.ClearFolders();
            var testFolder = WindsorContainer.Resolve<IFolder>(new { path = testFolderPath });
            appState.AddFolder(testFolder);
            var filesCount = testFolder.Files.Count;

            WindsorContainer.Release(appState);
            var appStateCurrent = TransientApplicationState(WindsorContainer);

            Assert.IsNotNull(appStateCurrent.Folders.First().Files);
            Assert.IsTrue(appStateCurrent.Folders.First().IsValid);
        }

        [TestMethod]
        public void TestApplicationStateRestoredModelReadsFiles()
        {
            var testFolderPath = UnitTestFolder.RealTestPath;
            var appState = TransientApplicationState(WindsorContainer);
            appState.ClearFolders();
            var testFolder = WindsorContainer.Resolve<IFolder>(new { path = testFolderPath });
            appState.AddFolder(testFolder);
            var filesCount = testFolder.Files.Count;

            WindsorContainer.Release(appState);
            var appStateCurrent = TransientApplicationState(WindsorContainer);

            Assert.IsNotNull(appStateCurrent.Folders.First().Files);
            Assert.AreEqual(filesCount, appStateCurrent.Folders.First().Files.Count);
        }


        public static IApplicationState TransientApplicationState(IWindsorContainer windsorContainer)
        {
            var result = windsorContainer.Resolve<IApplicationState>();
            
            return result;
        }
    }
}
