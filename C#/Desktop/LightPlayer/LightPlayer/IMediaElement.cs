
using System;
namespace LightPlayer
{
    public interface IMediaElement
    {
        Action Play { get; }

        Action Stop { get; }
    }
}
