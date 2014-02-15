using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace LightPlayer
{
    public class Folder : IFolder
    {
        private string path;

        private IFileMask fileMask;

        private Folder()
        {
        }

        public Folder(string path, IFileMask fileMask)
        {
            this.fileMask = fileMask;
            Path = path;
        }

        public bool IsValid
        {
            get { return Directory.Exists(Path); }
        }

        public IFileMask FileMask
        {
            get
            {
                return fileMask ?? ViewModelLocator.DependecyContainer.Resolve<IFileMask>();
            }
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
            fileSystemWatcher.Created += OnFileCreated;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (!FileMask.IsVisible(e.FullPath))
            {
                return;
            }

            Files.Add(e.FullPath);
        }

        private void SetupFilesCollection()
        {
            var fileNames = IsValid ? Directory.GetFiles(Path) : Enumerable.Empty<string>();
            Files = new ObservableCollection<string>(fileNames.Where(f => FileMask.IsVisible(f)).ToList());
        }
    }
}
