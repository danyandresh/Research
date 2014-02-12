using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace LightPlayer
{
    public class SelectDialog : ISelectDialog
    {
        private CommonOpenFileDialog openFileDialog;
        private Func<Tuple<CommonFileDialogResult, string>> selectFolder;

        public SelectDialog(CommonOpenFileDialog openFileDialog)
        {
            this.openFileDialog = openFileDialog;
            selectFolder = () =>
                {
                    var dialogResult = openFileDialog.ShowDialog();
                    string path = null;

                    if (CommonFileDialogResult.Ok == dialogResult)
                    {
                        path=openFileDialog.FileName;
                    }

                    return new Tuple<CommonFileDialogResult, string>(dialogResult, path);
                };
        }

        public SelectDialog(Func<Tuple<CommonFileDialogResult, string>> selectFolder)
        {
            this.selectFolder = selectFolder;
        }

        public Tuple<DialogResult, string> Show()
        {
            var dialogResult = selectFolder();

            return dialogResult.Item1 == CommonFileDialogResult.Ok ?
                new Tuple<DialogResult, string>(DialogResult.Ok, dialogResult.Item2) :
                new Tuple<DialogResult, string>(DialogResult.Cancel, string.Empty);

        }
    }
}
