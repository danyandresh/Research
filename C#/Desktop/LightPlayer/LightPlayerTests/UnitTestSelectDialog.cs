using LightPlayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;

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
            var folderSelectDialog = WindsorContainer.Resolve<CommonOpenFileDialog>("openFolderDialog");
            Assert.IsTrue(folderSelectDialog.IsFolderPicker);
        }

        [TestMethod]
        public void TestSelectDialogDependendsOnCommonFileDialog()
        {
            var realFolder = UnitTestFolder.RealTestPath;

            var openFolderDialog = WindsorContainer.Resolve<CommonOpenFileDialog>("openFolderDialog");
            Assert.IsNotNull(openFolderDialog);
            var folderSelectDialog = WindsorContainer.Resolve<ISelectDialog>("folderSelectDialog", new { openFileDialog = openFolderDialog });
            Assert.IsNotNull(folderSelectDialog);
        }

        [TestMethod]
        public void TestSelectDialogBuildRightAroundFileDialogResponsOk()
        {
            var realFolder = UnitTestFolder.RealTestPath;


            Func<Tuple<CommonFileDialogResult, string>> testSelector = () =>
            {
                return new Tuple<CommonFileDialogResult, string>(CommonFileDialogResult.Ok, realFolder);
            };

            // Use the wanted constructor directly as WindsorCastle has a problem selecting the right one (when constructors have the same number of parameters)
            ISelectDialog folderSelectDialog = new SelectDialog(testSelector);
            var dialogResult = folderSelectDialog.Show();

            Assert.AreEqual(DialogResult.Ok, dialogResult.Item1, "SelectDialog does not return correct dialog result");
            Assert.AreEqual(realFolder, dialogResult.Item2, "FileDialog should have returned the real folder");
        }

        [TestMethod]
        public void TestSelectDialogBuildRightAroundFileDialogResponsCancel()
        {
            var realFolder = UnitTestFolder.RealTestPath;


            Func<Tuple<CommonFileDialogResult, string>> testSelector = () =>
            {
                return new Tuple<CommonFileDialogResult, string>(CommonFileDialogResult.Cancel, realFolder);
            };

            // Use the wanted constructor directly as WindsorCastle has a problem selecting the right one (when constructors have the same number of parameters)
            ISelectDialog folderSelectDialog = new SelectDialog(testSelector);
            var dialogResult = folderSelectDialog.Show();

            Assert.AreEqual(DialogResult.Cancel, dialogResult.Item1, "SelectDialog does not return correct dialog result");
            Assert.AreEqual(string.Empty, dialogResult.Item2, "FileDialog should have returned the real folder");
        }

        [TestMethod]
        public void TestSelectDialogBuildRightAroundFileDialogResponsNone()
        {
            var realFolder = UnitTestFolder.RealTestPath;


            Func<Tuple<CommonFileDialogResult, string>> testSelector = () =>
            {
                return new Tuple<CommonFileDialogResult, string>(CommonFileDialogResult.None, realFolder);
            };

            // Use the wanted constructor directly as WindsorCastle has a problem selecting the right one (when constructors have the same number of parameters)
            ISelectDialog folderSelectDialog = new SelectDialog(testSelector);
            var dialogResult = folderSelectDialog.Show();

            Assert.AreEqual(DialogResult.Cancel, dialogResult.Item1, "SelectDialog does not return correct dialog result");
            Assert.AreEqual(string.Empty, dialogResult.Item2, "FileDialog should have returned the real folder");
        }
    }

}
