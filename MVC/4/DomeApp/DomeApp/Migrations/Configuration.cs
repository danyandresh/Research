namespace DomeApp.Migrations
{
    using DomeApp.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DomeAppContext>
    {
        public Configuration()
        {
            // Kee this false on deployment
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DomeAppContext context)
        {
            context.BlogPosts.AddOrUpdate(r => r.Title,
                                new BlogPost { Title = "DomeApp", Excerpt = "DomeApp is a demo application to research and leverage MVC4 features and ultimately end up with a _functional_ application.", Content = "DomeApp is a demo application to research and leverage MVC4 features and ultimately end up with a _functional_ application. Localization, package management, codefirst, mvc, razor engine are among the features exposed so far" }
                );

        }

        
    }
}
