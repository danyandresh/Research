using System.Collections.ObjectModel;

namespace LightPlayer
{
    public interface IApplicationState
    {
        ObservableCollection<IFolder> Folders { get; }
    }
}
