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
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(DomeAppContext context)
        {
        }
    }
}
