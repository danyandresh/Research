
namespace LightPlayer
{
    public class ViewModelLocator
    {
        public static IFolderViewModel FoldersViewModel { get { return new FolderViewModel(null); } }
    }
}
