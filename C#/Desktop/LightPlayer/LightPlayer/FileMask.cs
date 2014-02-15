
namespace LightPlayer
{
    class FileMask : IFileMask
    {
        public bool IsVisible(string fileName)
        {
            return fileName.EndsWith(".mp3");
        }
    }
}
