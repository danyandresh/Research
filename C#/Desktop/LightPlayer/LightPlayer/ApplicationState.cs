using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LightPlayer
{
    public class ApplicationState : DbContext, IApplicationState
    {
        public ApplicationState()
        {
        }

        public ApplicationState(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public DbSet<Folder> FolderSet { get; set; }

        public IEnumerable<IFolder> Folders { get { return FolderSet; } }

        public void AddFolder(IFolder folder)
        {
            if (FolderSet.Any(f => f.Path == folder.Path))
            {
                return;
            }

            FolderSet.Add((Folder)folder);
            SaveChanges();
        }

        public void ClearFolders()
        {
            FolderSet.ToList().ForEach(f => FolderSet.Remove(f));
            SaveChanges();
        }
    }
}
