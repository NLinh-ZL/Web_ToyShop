namespace ToyShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeCard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.Cards", new[] { "Product_ProductID" });
            DropColumn("dbo.Cards", "ProductID");
            RenameColumn(table: "dbo.Cards", name: "Product_ProductID", newName: "ProductID");
            AlterColumn("dbo.Cards", "ProductID", c => c.Long(nullable: false));
            AlterColumn("dbo.Cards", "ProductID", c => c.Long(nullable: false));
            CreateIndex("dbo.Cards", "ProductID");
            AddForeignKey("dbo.Cards", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "ProductID", "dbo.Products");
            DropIndex("dbo.Cards", new[] { "ProductID" });
            AlterColumn("dbo.Cards", "ProductID", c => c.Long());
            AlterColumn("dbo.Cards", "ProductID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Cards", name: "ProductID", newName: "Product_ProductID");
            AddColumn("dbo.Cards", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.Cards", "Product_ProductID");
            AddForeignKey("dbo.Cards", "Product_ProductID", "dbo.Products", "ProductID");
        }
    }
}
