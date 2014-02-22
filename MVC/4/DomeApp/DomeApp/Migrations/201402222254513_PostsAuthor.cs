namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PostsAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Author_UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPosts", "Author_UserId");
        }

        public override void Down()
        {
            DropIndex("dbo.BlogPosts", new[] { "Author_UserId" });
            DropColumn("dbo.BlogPosts", "Author_UserId");
        }
    }
}
