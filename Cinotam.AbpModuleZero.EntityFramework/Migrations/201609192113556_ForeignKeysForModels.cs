namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeysForModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chunks", "ContentObj_Id", "dbo.Contents");
            DropForeignKey("dbo.MenuContents", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Resources", "TemplateObj_Id", "dbo.Templates");
            DropIndex("dbo.Chunks", new[] { "ContentObj_Id" });
            DropIndex("dbo.MenuContents", new[] { "Menu_Id" });
            DropIndex("dbo.Resources", new[] { "TemplateObj_Id" });
            RenameColumn(table: "dbo.Chunks", name: "ContentObj_Id", newName: "ContentId");
            RenameColumn(table: "dbo.MenuContents", name: "Menu_Id", newName: "MenuId");
            RenameColumn(table: "dbo.Resources", name: "TemplateObj_Id", newName: "TemplateId");
            AlterColumn("dbo.Chunks", "ContentId", c => c.Int(nullable: false));
            AlterColumn("dbo.MenuContents", "MenuId", c => c.Int(nullable: false));
            AlterColumn("dbo.Resources", "TemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.Chunks", "ContentId");
            CreateIndex("dbo.MenuContents", "MenuId");
            CreateIndex("dbo.Resources", "TemplateId");
            AddForeignKey("dbo.Chunks", "ContentId", "dbo.Contents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuContents", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Resources", "TemplateId", "dbo.Templates", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.MenuContents", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.Chunks", "ContentId", "dbo.Contents");
            DropIndex("dbo.Resources", new[] { "TemplateId" });
            DropIndex("dbo.MenuContents", new[] { "MenuId" });
            DropIndex("dbo.Chunks", new[] { "ContentId" });
            AlterColumn("dbo.Resources", "TemplateId", c => c.Int());
            AlterColumn("dbo.MenuContents", "MenuId", c => c.Int());
            AlterColumn("dbo.Chunks", "ContentId", c => c.Int());
            RenameColumn(table: "dbo.Resources", name: "TemplateId", newName: "TemplateObj_Id");
            RenameColumn(table: "dbo.MenuContents", name: "MenuId", newName: "Menu_Id");
            RenameColumn(table: "dbo.Chunks", name: "ContentId", newName: "ContentObj_Id");
            CreateIndex("dbo.Resources", "TemplateObj_Id");
            CreateIndex("dbo.MenuContents", "Menu_Id");
            CreateIndex("dbo.Chunks", "ContentObj_Id");
            AddForeignKey("dbo.Resources", "TemplateObj_Id", "dbo.Templates", "Id");
            AddForeignKey("dbo.MenuContents", "Menu_Id", "dbo.Menus", "Id");
            AddForeignKey("dbo.Chunks", "ContentObj_Id", "dbo.Contents", "Id");
        }
    }
}
