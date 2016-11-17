namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserShouldChangePassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "ShouldChangePasswordOnLogin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "ShouldChangePasswordOnLogin");
        }
    }
}
