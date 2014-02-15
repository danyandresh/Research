using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System;

namespace LightPlayer
{
    public class PlaylistViewModel : IPlaylistViewModel
    {
        private string currentlyPlaying;

        public PlaylistViewModel(IFolder toPlay)
        {
            Folder = toPlay;
            currentlyPlaying = Folder.Files.FirstOrDefault();
        }

        public IFolder Folder { get; set; }

        public ICommand CommandPlayFile { get { return new DelegateCommand<Tuple<IMediaElement, string>>(PlayFile); } }

        public string CurrentlyPlaying
        {
            get { return currentlyPlaying; }
            set
            {
                currentlyPlaying = value;
                NotifyPropertyChanged("CurrentlyPlaying");
            }
        }

        private void PlayFile(Tuple<IMediaElement, string> @params)
        {
            @params.Item1.Stop();
            CurrentlyPlaying = @params.Item2;
            @params.Item1.Play();
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
