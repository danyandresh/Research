using System.Collections.Generic;

namespace LightPlayer
{
    public class FolderViewModel : IFolderViewModel
    {
        private IList<IFolder> models = new List<IFolder>();

        public void Add(IFolder folder)
        {
            if (folder != null && !folder.IsValid)
            {
                return;
            }

            models.Add(folder);
        }

        public IEnumerable<IFolder> Models { get { return models; } }
    }
}
