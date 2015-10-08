﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using LightPlayer.Conventions;
using LightPlayer.Converters;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LightPlayer
{
    public class ViewModelLocator
    {
        private static IWindsorContainer dependencyContainer;

        public static IFolderViewModel FoldersViewModel { get { return DependecyContainer.Resolve<IFolderViewModel>(); } }

        public static IPlaylistViewModel PlaylistViewModel { get { return DependecyContainer.Resolve<IPlaylistViewModel>(); } }

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

            container.Register(Component.For<CommonOpenFileDialog>().UsingFactoryMethod((k, context) =>
            {
                var fileDialog = new CommonOpenFileDialog();
                fileDialog.IsFolderPicker = true;
                return fileDialog;
            }).Named("openFolderDialog"));

            container.Register(Component.For<IFolder>().ImplementedBy<Folder>().LifestyleTransient());
            container.Register(Component.For<ISelectDialog>().ImplementedBy<SelectDialog>().Named("folderSelectDialog"));

            container.Register(Component.For<IApplicationState>().ImplementedBy<ApplicationState>()
                .DependsOn(Dependency.OnConfigValue("nameOrConnectionString", "LightPlayer.ApplicationStateConnection"))
                .Named("applicationState"));

            container.Register(Component
                .For<IFolderViewModel>()
                .ImplementedBy<FolderViewModel>()
                .DependsOn(Dependency.OnComponent(typeof(ISelectDialog), "folderSelectDialog"))
                .DependsOn(Dependency.OnComponent(typeof(ApplicationState), "applicationState")));

            container.Register(Component
                .For<IPlaylist>()
                .ImplementedBy<Playlist>()
                .LifestyleTransient());

            container.Kernel.Resolver.AddSubResolver(new PlaylistDependencyResolver(container.Kernel));

            container.Register(Component
                .For<IPlaylistViewModel>()
                .ImplementedBy<PlaylistViewModel>()
                .LifestyleTransient());

            container.Register(Component.For<MediaElementStringParamsTuple>());
            container.Register(Component.For<MediaInteractionConverter>());

            container.Register(Component.For<IFileMask>().ImplementedBy<FileMask>());

            return container;
        }
    }
}
