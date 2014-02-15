
using System.ComponentModel;
using System.Windows.Input;
namespace LightPlayer
{
    public interface IPlaylistViewModel : INotifyPropertyChanged
    {
        IFolder Folder { get; }

        string CurrentlyPlaying { get; set; }

        ICommand CommandPlayFile { get; }
    }
}
