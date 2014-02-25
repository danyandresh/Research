using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace LightPlayer
{
    public class Folder : IFolder
    {
        private string path;

        private IFileMask fileMask;

        private Dispatcher dispatcher;

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

                SetupFileWatcher();
            }
        }

        private ObservableCollection<string> files;

        public ObservableCollection<string> Files
        {
            get
            {
                if (files == null)
                {
                    files = SetupFilesCollection();
                }

                return files;
            }

            private set { }
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

        public Dispatcher GetDispatcher()
        {
            return dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (!FileMask.IsVisible(e.FullPath))
            {
                return;
            }

            GetDispatcher().Invoke(() =>
            {
                Files.Add(e.FullPath);
            });
        }

        private ObservableCollection<string> SetupFilesCollection()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            var fileNames = IsValid ? Directory.GetFiles(Path) : Enumerable.Empty<string>();
            return new ObservableCollection<string>(fileNames.Where(f => FileMask.IsVisible(f)).ToList());
        }
    }
}
