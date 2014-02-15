using System;
using System.Collections.Generic;

namespace LightPlayer
{
    class Playlist : IPlaylist
    {
        private IFolder folder;

        private Queue<string> playQueue;

        public Playlist(IFolder folder)
        {
            this.folder = folder;
            playQueue = new Queue<string>(folder.Files);
            MoveNext();
        }

        public string CurrentFile
        {
            get;
            private set;
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
