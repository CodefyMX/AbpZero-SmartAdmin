namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.MenuSections", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.MenuSectionItems", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuSectionItems", "Order");
            DropColumn("dbo.MenuSections", "Order");
            DropColumn("dbo.Menus", "Order");
        }
    }
}
