
using System;
namespace LightPlayer
{
    public interface IMediaElement
    {
        Func<int> CalculateDuration { get; }

        Func<int> CalculatePosition { get; }

        Action Play { get; }

        Action Stop { get; }

        Action Pause { get; }

        Action<int> Seek { get; }
    }
}
