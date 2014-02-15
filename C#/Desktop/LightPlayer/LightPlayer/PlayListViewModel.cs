
namespace LightPlayer
{
    public class PlaylistViewModel : IPlaylistViewModel
    {
        public PlaylistViewModel(IFolder toPlay)
        {
            Folder = toPlay;
        }

        public IFolder Folder { get; set; }
    }
}
