using System;
using System.Collections.Generic;

namespace LightPlayer
{
    class Playlist : IPlaylist
    {
        private Queue<string> playQueue;

        public Playlist(IFolder folder)
        {
            Folder = folder;
            playQueue = new Queue<string>(folder.Files);
            MoveNext();
        }

        public IFolder Folder { get; private set; }

        public string CurrentFile
        {
            get;
            set;
        }

        public bool MoveNext()
        {
            if (playQueue.Count == 0)
            {
                return false;
            }

            CurrentFile = playQueue.Dequeue();
            playQueue.Enqueue(CurrentFile);

            return true; ;
        }
    }
}
