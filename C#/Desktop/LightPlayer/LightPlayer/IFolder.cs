using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LightPlayer
{
    public interface IFolder
    {
        bool IsValid { get; }

        string Path { get; }

        ObservableCollection<string> Files { get; }
    }
}
