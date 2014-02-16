using System.ComponentModel;
using System.Windows.Input;

namespace LightPlayer
{
    public interface IPlaylistViewModel : INotifyPropertyChanged
    {
        IPlaylist Playlist { get; }

        string CurrentlyPlaying { get; set; }

        ICommand CommandPlayFile { get; }

        ICommand CommandPlay { get; }

        ICommand CommandPlayNext { get; }

        ICommand CommandStop { get; }
    }
}
