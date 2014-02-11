using System.Collections.Generic;

namespace LightPlayer
{
    public interface IFolder
    {
        bool IsValid { get; }

        string Path { get; }

        IEnumerable<string> Files { get; }
    }
}
