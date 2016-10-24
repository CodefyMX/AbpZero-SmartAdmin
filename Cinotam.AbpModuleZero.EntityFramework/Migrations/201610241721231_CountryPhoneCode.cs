namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryPhoneCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "CountryPhoneCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "CountryPhoneCode");
        }
    }
}
