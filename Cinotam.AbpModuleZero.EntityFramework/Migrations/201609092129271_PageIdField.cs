namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageIdField : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contents", "Page_Id", "dbo.Pages");
            DropIndex("dbo.Contents", new[] { "Page_Id" });
            RenameColumn(table: "dbo.Contents", name: "Page_Id", newName: "PageId");
            AlterColumn("dbo.Contents", "PageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contents", "PageId");
            AddForeignKey("dbo.Contents", "PageId", "dbo.Pages", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contents", "PageId", "dbo.Pages");
            DropIndex("dbo.Contents", new[] { "PageId" });
            AlterColumn("dbo.Contents", "PageId", c => c.Int());
            RenameColumn(table: "dbo.Contents", name: "PageId", newName: "Page_Id");
            CreateIndex("dbo.Contents", "Page_Id");
            AddForeignKey("dbo.Contents", "Page_Id", "dbo.Pages", "Id");
        }
    }
}
