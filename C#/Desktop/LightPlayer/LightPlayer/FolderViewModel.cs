using Castle.Windsor;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LightPlayer
{
    public class FolderViewModel : IFolderViewModel
    {
        private ISelectDialog selectDialog;

        public FolderViewModel(ISelectDialog selectDialog)
        {
            this.selectDialog = selectDialog;
        }

        private ObservableCollection<IFolder> models = new ObservableCollection<IFolder>();

        public void Add(IFolder folder)
        {
            if (folder != null && !folder.IsValid)
            {
                return;
            }

            models.Add(folder);
        }

        public ObservableCollection<IFolder> Models { get { return models; } }

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
    }
}
