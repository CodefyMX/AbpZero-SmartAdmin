namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuSections", "CategoryId", c => c.Int());
            AddColumn("dbo.MenuSectionItems", "PageId", c => c.Int());
            CreateIndex("dbo.MenuSections", "CategoryId");
            CreateIndex("dbo.MenuSectionItemContents", "PageId");
            AddForeignKey("dbo.MenuSections", "CategoryId", "dbo.Categories", "Id");
            AddForeignKey("dbo.MenuSectionItemContents", "PageId", "dbo.Pages", "Id");
            DropColumn("dbo.MenuSections", "CategoryDiscriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuSections", "CategoryDiscriminator", c => c.String());
            DropForeignKey("dbo.MenuSectionItemContents", "PageId", "dbo.Pages");
            DropForeignKey("dbo.MenuSections", "CategoryId", "dbo.Categories");
            DropIndex("dbo.MenuSectionItemContents", new[] { "PageId" });
            DropIndex("dbo.MenuSections", new[] { "CategoryId" });
            DropColumn("dbo.MenuSectionItems", "PageId");
            DropColumn("dbo.MenuSections", "CategoryId");
        }
    }
}
