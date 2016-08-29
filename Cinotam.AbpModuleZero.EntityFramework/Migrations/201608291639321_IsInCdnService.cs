namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsInCdnService : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "IsPictureOnCdn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "IsPictureOnCdn");
        }
    }
}
