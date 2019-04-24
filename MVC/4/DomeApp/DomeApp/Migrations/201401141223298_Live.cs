namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Live : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "Author_UserId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Comments", "Author_UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Comments", "Author_UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "Author_UserId" });
            DropForeignKey("dbo.Comments", "Author_UserId", "dbo.UserProfile");
            DropColumn("dbo.Comments", "Author_UserId");
            DropColumn("dbo.Comments", "CreatedDate");
            DropTable("dbo.UserProfile");
        }
    }
}
