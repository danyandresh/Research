using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;
using LightPlayer;


namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestLocators : TestContext
    {
        [TestMethod]
        public void TestLocatorProvidesFoldersVM()
        {
            Assert.IsNotNull(ViewModelLocator.FoldersViewModel, "ViewModelLocator does not create folders VM");
        }
    }
}
