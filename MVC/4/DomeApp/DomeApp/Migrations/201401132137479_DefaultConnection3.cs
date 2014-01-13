namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultConnection3 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserProfile");
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Comments", "User_UserId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Comments", "User_UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Comments", "User_UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "User_UserId" });
            DropForeignKey("dbo.Comments", "User_UserId", "dbo.UserProfile");
            DropColumn("dbo.Comments", "User_UserId");
            DropTable("dbo.UserProfile");
        }
    }
}
