using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace LightPlayer.Conventions
{
    class PlaylistDependencyResolver : ISubDependencyResolver
    {
        private IKernel kernel;

        public PlaylistDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return model.Implementation == typeof(PlaylistViewModel) && dependency.DependencyKey == "toPlay";
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            var vm = kernel.Resolve<IFolderViewModel>();
            return vm.SelectedFolder;
        }
    }
}
