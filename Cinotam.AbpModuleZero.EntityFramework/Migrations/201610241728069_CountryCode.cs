namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "CountryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "CountryCode");
        }
    }
}
