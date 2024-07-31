namespace ToyShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Product_ProductID = c.Long(),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.Products", t => t.Product_ProductID)
                .Index(t => t.Product_ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.Cards", new[] { "Product_ProductID" });
            DropTable("dbo.Cards");
        }
    }
}
