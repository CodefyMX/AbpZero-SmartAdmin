namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainPageProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "IsMainPage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "IsMainPage");
        }
    }
}
