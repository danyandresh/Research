using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System;

namespace LightPlayer
{
    public class PlaylistViewModel : IPlaylistViewModel
    {
        public PlaylistViewModel(IPlaylist playlist)
        {
        }

        public IPlaylist Playlist { get; set; }

        public ICommand CommandPlayFile { get { return new DelegateCommand<Tuple<IMediaElement, string>>(PlayFile); } }

        public ICommand CommandPlay
        {
            get
            {
                return new DelegateCommand<IMediaElement>((mediaElement) => { mediaElement.Play(); });
            }
        }

        public ICommand CommandPlayNext
        {
            get
            {
                return new DelegateCommand<IMediaElement>((mediaElement) =>
                {
                    Playlist.MoveNext();
                    CurrentlyPlaying = Playlist.CurrentFile;
                    mediaElement.Play();
                });
            }
        }

        public ICommand CommandStop
        {
            get
            {
                return new DelegateCommand<IMediaElement>((mediaElement) => { mediaElement.Stop(); });
            }
        }

        public ICommand CommandPause
        {
            get
            {
                return new DelegateCommand<IMediaElement>((mediaElement) => { mediaElement.Pause(); });
            }
        }

        public string CurrentlyPlaying
        {
            get { return Playlist.CurrentFile; }
            set
            {
                Playlist.CurrentFile = value;
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
