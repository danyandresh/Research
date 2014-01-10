namespace DomeApp.Migrations
{
    using DomeApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DomeApp.Models.DomeAppContext>
    {
        public Configuration()
        {
            // Kee this false on deployment
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DomeApp.Models.DomeAppContext context)
        {
            context.BlogPosts.AddOrUpdate(r => r.Title,
                                new BlogPost { Title = "Postul 34", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  5", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 22", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 91", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 58", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 37", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 44", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 57", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 59", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  3", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 66", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 41", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 84", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 39", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 79", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 93", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 20", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 98", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 42", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 10", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 13", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 16", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 18", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  6", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 54", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 76", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 85", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 17", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 99", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 19", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 52", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 88", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 63", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 83", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 46", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 35", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 70", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 77", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 72", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 24", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 71", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  9", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 73", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  2", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  8", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 32", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 45", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 50", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 65", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 38", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 74", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 89", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 94", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 81", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul  7", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 68", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 82", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 28", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 95", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 80", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 36", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 47", Content = "Lorem impsum dolor sit a met" },
                new BlogPost { Title = "Postul 53", Content = "Lorem impsum dolor sit a met" }
                );
        }
    }
}
