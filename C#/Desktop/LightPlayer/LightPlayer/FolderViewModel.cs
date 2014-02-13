using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LightPlayer
{
    public class FolderViewModel : IFolderViewModel
    {
        private ISelectDialog selectDialog;
        private IApplicationState applicationState;

        public FolderViewModel(ISelectDialog selectDialog, IApplicationState appState)
        {
            this.selectDialog = selectDialog;
            applicationState = appState;
            Models = new ObservableCollection<IFolder>(appState.Folders);
        }

        public void Add(IFolder folder)
        {
            if (folder != null && !folder.IsValid)
            {
                return;
            }

            applicationState.AddFolder(folder);
            Models.Add(folder);
        }

        public ObservableCollection<IFolder> Models { get; private set; }

        public ICommand CommandAddFolder { get { return new DelegateCommand(SelectFolder); } }

        private void SelectFolder()
        {
            var dialogResult = selectDialog.Show();
            if (dialogResult.Item1 == DialogResult.Cancel)
            {
                return;
            }

            Add(new Folder(dialogResult.Item2));
        }


        public void Clear()
        {
            Models.Clear();
            applicationState.ClearFolders();
        }
    }
}
