using System.IO;

namespace LightPlayer
{
    public class Folder : IFolder
    {
        public Folder(string path)
        {
            Path = path;
        }

        public bool IsValid
        {
            get { return Directory.Exists(Path); }
        }

        public string Path
        {
            get;
            private set;
        }
    }
}
