using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LightPlayer
{
    public interface IFolder
    {
        bool IsValid { get; }

        string Path { get; }

        ObservableCollection<string> Files { get; }

        void OnFileCreated(object sender, FileSystemEventArgs e);
    }
}
