using System.ComponentModel;
using System.Windows.Input;

namespace LightPlayer
{
    public interface IPlaylistViewModel : INotifyPropertyChanged
    {
        IPlaylist Playlist { get; }

        string CurrentlyPlaying { get; set; }

        int Duration { get; }

        int Position { get; }

        ICommand CommandPlayFile { get; }

        ICommand CommandPlay { get; }

        ICommand CommandPlayNext { get; }

        ICommand CommandStop { get; }

        ICommand CommandPause { get; }

        ICommand CommandSeek { get; }

        ICommand CommandMediaOpened { get; }

        ICommand CommandToggleBackgroundOperations { get; }
    }
}
