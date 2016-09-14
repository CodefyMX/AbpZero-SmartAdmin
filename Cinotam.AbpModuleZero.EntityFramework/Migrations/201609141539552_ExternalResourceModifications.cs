namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalResourceModifications : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Resources", name: "Template_Id", newName: "TemplateObj_Id");
            RenameIndex(table: "dbo.Resources", name: "IX_Template_Id", newName: "IX_TemplateObj_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Resources", name: "IX_TemplateObj_Id", newName: "IX_Template_Id");
            RenameColumn(table: "dbo.Resources", name: "TemplateObj_Id", newName: "Template_Id");
        }
    }
}
