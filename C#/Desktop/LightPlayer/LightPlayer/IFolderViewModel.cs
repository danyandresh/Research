using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LightPlayer
{
    public interface IFolderViewModel
    {
        void Add(IFolder folder);

        ObservableCollection<IFolder> Models { get; }

        IFolder SelectedFolder { get; }

        ICommand CommandAddFolder { get; }

        ICommand CommandSelectFolder { get; }

        void Clear();
    }
}
