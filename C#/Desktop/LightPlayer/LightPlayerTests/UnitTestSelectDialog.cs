using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestSelectDialog : TestContext
    {
        /// Memento: It is impossible to force functionality chaining by TDD between CommonFileDialog and SelectDialog as CommonFileDialog does not respect Liskov's substitution principle.

        [TestMethod]
        public void TestSelectDialogDependencyIsFolderPicker()
        {
            var realFolder = UnitTestFolder.RealTestPath;
            var folderSelectDialog = WindsorContainer.Resolve<CommonOpenFileDialog>("folderSelectDialog");
            Assert.IsTrue(folderSelectDialog.IsFolderPicker);
        }
    }

}
