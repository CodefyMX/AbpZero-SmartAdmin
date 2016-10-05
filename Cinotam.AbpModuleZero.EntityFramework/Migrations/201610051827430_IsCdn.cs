namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCdn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "IsCdn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "IsCdn");
        }
    }
}
