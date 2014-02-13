using System.Collections.ObjectModel;

namespace LightPlayer
{
    public class ApplicationState : IApplicationState
    {
        public ApplicationState()
        {
            Folders = new ObservableCollection<IFolder>();
        }

        public ObservableCollection<IFolder> Folders { get; private set; }
    }
}
