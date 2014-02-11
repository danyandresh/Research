using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    }
}
