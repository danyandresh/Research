using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace LightPlayer
{
    public class Folder : IFolder
    {
        private string path;

        private Folder()
        {
        }

        public Folder(string path)
        {
            Path = path;
        }

        public bool IsValid
        {
            get { return Directory.Exists(Path); }
        }

        [Key]
        public string Path
        {
            get { return path; }
            private set
            {
                path = value;

                SetupFilesCollection();
                //TODO Between these two operations a file could be added to the folder and be missed from the list of files _visible_
                SetupFileWatcher();
            }
        }


        public ObservableCollection<string> Files
        {
            get;
            private set;
        }

        private void SetupFileWatcher()
        {
            if (!IsValid)
            {
                return;
            }

            var fileSystemWatcher = new FileSystemWatcher(Path);
            fileSystemWatcher.Created += (sender, e) =>
                {
                    Files.Add(e.FullPath);
                };

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void SetupFilesCollection()
        {
            var fileNames = IsValid ? Directory.GetFiles(Path) : Enumerable.Empty<string>();
            Files = new ObservableCollection<string>(fileNames);
        }
    }
}
