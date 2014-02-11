using System.Collections.Generic;
using System.IO;
using System.Linq;

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


        public IEnumerable<string> Files
        {
            get { return IsValid ? Directory.GetFiles(Path) : Enumerable.Empty<string>(); }
        }
    }
}
