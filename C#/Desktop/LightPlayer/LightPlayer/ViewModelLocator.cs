
using Castle.MicroKernel.Registration;
using Castle.Windsor;
namespace LightPlayer
{
    public class ViewModelLocator
    {
        private static IWindsorContainer dependencyContainer;

        public static IFolderViewModel FoldersViewModel { get { return new FolderViewModel(null); } }

        public static IWindsorContainer DependecyContainer
        {
            get
            {
                if (dependencyContainer == null)
                {
                    dependencyContainer = SetupDependencyContainer();
                }

                return dependencyContainer;
            }
        }

        public static IWindsorContainer SetupDependencyContainer()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ISelectDialog>().ImplementedBy<SelectDialog>());

            container.Register(Component
                .For<IFolderViewModel>()
                .ImplementedBy<FolderViewModel>()
                .DependsOn(Dependency.OnValue<ISelectDialog>(container.Resolve<ISelectDialog>())));

            return container;
        }
    }
}
