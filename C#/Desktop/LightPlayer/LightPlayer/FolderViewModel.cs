using System.Collections.Generic;

namespace LightPlayer
{
    public class FolderViewModel : IFolderViewModel
    {
        private IList<IFolder> models = new List<IFolder>();

        public void Add(IFolder folder)
        {
            models.Add(folder);
        }

        public IEnumerable<IFolder> Models { get { return models; } }
    }
}
