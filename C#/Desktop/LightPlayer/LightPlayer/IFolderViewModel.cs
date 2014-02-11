using System.Collections.Generic;

namespace LightPlayer
{
    public interface IFolderViewModel
    {
        void Add(IFolder folder);

        IEnumerable<IFolder> Models { get; }
    }
}
