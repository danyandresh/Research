using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LightPlayer
{
    public interface IFolderViewModel
    {
        void Add(IFolder folder);

        ObservableCollection<IFolder> Models { get; }

        ICommand CommandAddFolder { get; }
    }
}
