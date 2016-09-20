namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivableOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuSections", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuSectionItems", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuSectionItems", "IsActive");
            DropColumn("dbo.MenuSections", "IsActive");
            DropColumn("dbo.Menus", "IsActive");
        }
    }
}
