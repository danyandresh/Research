namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultConnection2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "BlogPost_Id", "dbo.BlogPosts");
            DropIndex("dbo.Comments", new[] { "BlogPost_Id" });
            AddColumn("dbo.Comments", "Post_Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.BlogPosts", "Id", cascadeDelete: true);
            CreateIndex("dbo.Comments", "Post_Id");
            DropColumn("dbo.Comments", "BlogPost_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "BlogPost_Id", c => c.Int());
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.BlogPosts");
            DropColumn("dbo.Comments", "Post_Id");
            CreateIndex("dbo.Comments", "BlogPost_Id");
            AddForeignKey("dbo.Comments", "BlogPost_Id", "dbo.BlogPosts", "Id");
        }
    }
}
