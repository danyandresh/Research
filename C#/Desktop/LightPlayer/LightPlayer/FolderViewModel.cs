using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LightPlayer
{
    public class FolderViewModel : IFolderViewModel
    {
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
            Add(new Folder("c:\\"));
        }
    }
}
