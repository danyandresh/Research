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
        public void TestApplicationStateCanBeDisposed()
        {
            var appStateInitial = TransientApplicationState(WindsorContainer);
            WindsorContainer.Release(appStateInitial);
            Assert.IsFalse(WindsorContainer.Kernel.ReleasePolicy.HasTrack(appStateInitial));

            var appStateCurrent = TransientApplicationState(WindsorContainer);

            Assert.AreNotEqual(appStateInitial, appStateCurrent, "ApplicationState should have been disposed");
        }

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


        public static IApplicationState TransientApplicationState(IWindsorContainer windsorContainer)
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IApplicationState>()
                .ImplementedBy<ApplicationState>()
                .LifestyleTransient()
            );

            windsorContainer.AddChildContainer(container);
            var result = container.Resolve<IApplicationState>();
            windsorContainer.RemoveChildContainer(container);
            //container.Dispose();
            return result;
        }
    }
}
