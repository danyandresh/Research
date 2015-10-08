using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Timers;
using System.Windows;

namespace LightPlayer
{
    public class PlaylistViewModel : IPlaylistViewModel
    {
        private Timer _mediaWatcher=new Timer();

        private volatile bool _backgroundOperationsSuspended;

        public PlaylistViewModel(IPlaylist playlist)
        {
        }

        private int _duration;

        private int _position;

        public IPlaylist Playlist { get; set; }

        public ICommand CommandPlayFile
        {
            get { return new DelegateCommand<Tuple<IMediaElement, string>>(PlayFile); }
        }

        public ICommand CommandPlay
        {
            get
            {
                return new DelegateCommand<IMediaElement>(
                    (mediaElement) =>
                    {
                        mediaElement.Play();
                        CalculateDurationAndPosition(mediaElement);
                    });
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
                    CalculateDurationAndPosition(mediaElement);
                });
            }
        }

        public ICommand CommandStop
        {
            get
            {
                return new DelegateCommand<IMediaElement>(
                    (mediaElement) =>
                    {
                        mediaElement.Stop();
                        CalculateDurationAndPosition(mediaElement);
                    });
            }
        }

        public ICommand CommandPause
        {
            get
            {
                return new DelegateCommand<IMediaElement>((mediaElement) => { mediaElement.Pause(); });
            }
        }

        public ICommand CommandSeek
        {
            get
            {
                var command = new DelegateCommand<Tuple<IMediaElement, int>>(Seek);

                return command;
            }
        }

        public ICommand CommandMediaOpened
        {
            get
            {
                return new DelegateCommand<IMediaElement>(
                    (mediaElement) =>
                    {
                        CalculateDurationAndPosition(mediaElement);
                        Watch(mediaElement);
                    });
            }
        }

        public ICommand CommandToggleBackgroundOperations
        {
            get
            {
                return new DelegateCommand<bool?>(
                    (enable) =>
                    {
                        _backgroundOperationsSuspended = enable == false;
                        if (_backgroundOperationsSuspended)
                        {
                            _mediaWatcher.Stop();
                        }
                        else
                        {
                            _mediaWatcher.Start();
                        }
                    });
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

        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                NotifyPropertyChanged("Duration");
            }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                NotifyPropertyChanged("Position");
            }
        }

        private void Watch(IMediaElement mediaElement)
        {
            if (_mediaWatcher != null)
            {
                _mediaWatcher.Stop();
                _mediaWatcher.Close();
            }

            _mediaWatcher = new Timer(500);
            _mediaWatcher.Elapsed += (sender, e) =>
            {
                if (_backgroundOperationsSuspended)
                {
                    return;
                }

                CalculateDurationAndPosition(mediaElement);
            };

            _mediaWatcher.Start();
        }

        private void CalculateDurationAndPosition(IMediaElement media)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        if (_backgroundOperationsSuspended)
                        {
                            return;
                        }

                        Duration = media.CalculateDuration();
                        Position = media.CalculatePosition();
                    }),
                null);

        }

        private void Seek(Tuple<IMediaElement, int> p)
        {
            if (Position == p.Item2)
            {
                return;
            }

            p.Item1.Seek(p.Item2);
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
