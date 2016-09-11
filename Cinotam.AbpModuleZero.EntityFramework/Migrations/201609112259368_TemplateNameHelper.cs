namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateNameHelper : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "TemplateUniqueName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "TemplateUniqueName");
        }
    }
}
