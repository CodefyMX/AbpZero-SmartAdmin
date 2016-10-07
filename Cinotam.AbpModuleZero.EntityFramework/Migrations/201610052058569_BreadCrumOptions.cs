namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BreadCrumOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "ShowBreadCrum", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pages", "BreadCrumInContainer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "BreadCrumInContainer");
            DropColumn("dbo.Pages", "ShowBreadCrum");
        }
    }
}
