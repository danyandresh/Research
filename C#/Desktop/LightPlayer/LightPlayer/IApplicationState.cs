using System.Collections.Generic;

namespace LightPlayer
{
    public interface IApplicationState
    {
        IEnumerable<IFolder> Folders { get; }

        void AddFolder(IFolder folder);

        void ClearFolders();
    }
}
