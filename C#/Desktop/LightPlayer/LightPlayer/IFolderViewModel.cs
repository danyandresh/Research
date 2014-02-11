using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LightPlayer
{
    public interface IFolderViewModel
    {
        void Add(IFolder folder);

        ObservableCollection<IFolder> Models { get; }
    }
}
