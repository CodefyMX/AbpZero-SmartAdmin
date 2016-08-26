namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "ProfilePicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "ProfilePicture");
        }
    }
}
