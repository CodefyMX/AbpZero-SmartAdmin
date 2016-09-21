namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuNameInCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuSections", "CategoryDiscriminator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuSections", "CategoryDiscriminator");
        }
    }
}
