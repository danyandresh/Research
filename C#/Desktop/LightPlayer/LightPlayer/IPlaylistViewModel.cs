
using System.ComponentModel;
namespace LightPlayer
{
    public interface IPlaylistViewModel : INotifyPropertyChanged
    {
        IFolder Folder { get; }

        string CurrentlyPlaying { get; set; }
    }
}
