namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredUserDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfile", "DisplayName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "DisplayName", c => c.String());
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
        }
    }
}
