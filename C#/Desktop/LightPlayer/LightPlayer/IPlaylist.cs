
namespace LightPlayer
{
    public interface IPlaylist
    {
        string CurrentFile { get; set; }

        bool MoveNext();

        IFolder Folder { get; }
    }
}
