using System;
using System.Windows.Controls;

namespace LightPlayer
{
    public class MediaElementProxy : IMediaElement
    {
        public Func<int> CalculateDuration { get; private set; }

        public Func<int> CalculatePosition { get; private set; }

        public Action<int> Seek { get; private set; }

        public Action Play { get; private set; }

        public Action Stop { get; private set; }

        public Action Pause { get; private set; }

        public static IMediaElement BuildProxy(MediaElement mediaElement)
        {
            var result = new MediaElementProxy();
            result.Play = mediaElement.Play;
            result.Stop = mediaElement.Stop;
            result.Pause = mediaElement.Pause;
            result.Seek = position => mediaElement.Position = TimeSpan.FromSeconds(position);

            result.CalculateDuration = () =>
            {
                var duration = 0;

                if (mediaElement.NaturalDuration.HasTimeSpan)
                {
                    duration = (int)mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                }

                return duration;
            };

            result.CalculatePosition = () =>
            {
                var position = (int)mediaElement.Position.TotalSeconds;

                return position;
            };

            return result;
        }
    }
}