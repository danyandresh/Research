
namespace LightPlayer
{
    public interface IPlaylist
    {
        string CurrentFile { get; }

        bool MoveNext();
    }
}
