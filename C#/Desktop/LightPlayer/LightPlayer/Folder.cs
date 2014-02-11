using System;

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
            get { throw new NotImplementedException(); }
        }

        public string Path
        {
            get;
            private set;
        }
    }
}
