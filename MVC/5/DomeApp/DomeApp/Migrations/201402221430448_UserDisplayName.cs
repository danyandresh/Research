namespace DomeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "DisplayName");
        }
    }
}
