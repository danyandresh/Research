using System.ComponentModel;

namespace LightPlayer
{
    public class PlaylistViewModel : IPlaylistViewModel
    {
        private string currentlyPlaying;

        public PlaylistViewModel(IFolder toPlay)
        {
            Folder = toPlay;
        }

        public IFolder Folder { get; set; }

        public string CurrentlyPlaying
        {
            get { return currentlyPlaying; }
            set
            {
                currentlyPlaying = value;
                NotifyPropertyChanged("CurrentlyPlaying");
            }
        }

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
