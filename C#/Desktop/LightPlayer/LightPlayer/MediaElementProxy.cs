using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightPlayer
{
    public class MediaElementProxy : IMediaElement
    {
        public Action Play { get; private set; }

        public Action Stop { get; private set; }

        public static IMediaElement BuildProxy(System.Windows.Controls.MediaElement mediaElement)
        {
            var result = new MediaElementProxy();
            result.Play = mediaElement.Play;
            result.Stop = mediaElement.Stop;

            return result;
        }
    }
}
