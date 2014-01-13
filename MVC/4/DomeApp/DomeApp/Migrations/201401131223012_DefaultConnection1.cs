namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultConnection1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "BlogPost_Id", c => c.Int());
            AddForeignKey("dbo.Comments", "BlogPost_Id", "dbo.BlogPosts", "Id");
            CreateIndex("dbo.Comments", "BlogPost_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "BlogPost_Id" });
            DropForeignKey("dbo.Comments", "BlogPost_Id", "dbo.BlogPosts");
            DropColumn("dbo.Comments", "BlogPost_Id");
        }
    }
}
