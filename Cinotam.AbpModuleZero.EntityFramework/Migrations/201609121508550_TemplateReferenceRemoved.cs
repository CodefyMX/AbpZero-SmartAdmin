namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateReferenceRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contents", "Template_Id", "dbo.Templates");
            DropForeignKey("dbo.Pages", "Template_Id", "dbo.Templates");
            DropIndex("dbo.Contents", new[] { "Template_Id" });
            DropIndex("dbo.Pages", new[] { "Template_Id" });
            AddColumn("dbo.Contents", "Title", c => c.String());
            AddColumn("dbo.Pages", "TemplateName", c => c.String());
            DropColumn("dbo.Contents", "Template_Id");
            DropColumn("dbo.Pages", "Template_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pages", "Template_Id", c => c.Int());
            AddColumn("dbo.Contents", "Template_Id", c => c.Int());
            DropColumn("dbo.Pages", "TemplateName");
            DropColumn("dbo.Contents", "Title");
            CreateIndex("dbo.Pages", "Template_Id");
            CreateIndex("dbo.Contents", "Template_Id");
            AddForeignKey("dbo.Pages", "Template_Id", "dbo.Templates", "Id");
            AddForeignKey("dbo.Contents", "Template_Id", "dbo.Templates", "Id");
        }
    }
}
