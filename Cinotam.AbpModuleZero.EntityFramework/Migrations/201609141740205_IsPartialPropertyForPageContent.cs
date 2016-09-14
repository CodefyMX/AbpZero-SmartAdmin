namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPartialPropertyForPageContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "IsPartial", c => c.Boolean(nullable: false));
            AddColumn("dbo.Templates", "IsPartial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "IsPartial");
            DropColumn("dbo.Contents", "IsPartial");
        }
    }
}
