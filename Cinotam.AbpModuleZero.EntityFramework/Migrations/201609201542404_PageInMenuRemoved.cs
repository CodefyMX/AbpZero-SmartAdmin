namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageInMenuRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menus", "Page_Id", "dbo.Pages");
            DropIndex("dbo.Menus", new[] { "Page_Id" });
            DropColumn("dbo.Menus", "Page_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Page_Id", c => c.Int());
            CreateIndex("dbo.Menus", "Page_Id");
            AddForeignKey("dbo.Menus", "Page_Id", "dbo.Pages", "Id");
        }
    }
}
