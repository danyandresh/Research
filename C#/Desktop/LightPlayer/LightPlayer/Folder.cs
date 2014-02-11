
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
            get { return true; }
        }

        public string Path
        {
            get;
            private set;
        }
    }
}
