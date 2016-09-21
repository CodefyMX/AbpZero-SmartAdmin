namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludePageInMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "IncludeInMenu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "IncludeInMenu");
        }
    }
}
