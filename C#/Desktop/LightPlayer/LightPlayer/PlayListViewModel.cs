﻿using Microsoft.Practices.Prism.Commands;
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
            currentlyPlaying = Folder.Files.First();
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