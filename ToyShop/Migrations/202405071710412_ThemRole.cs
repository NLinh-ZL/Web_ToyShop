namespace ToyShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemRole : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Role", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Role", c => c.String(nullable: false));
        }
    }
}
