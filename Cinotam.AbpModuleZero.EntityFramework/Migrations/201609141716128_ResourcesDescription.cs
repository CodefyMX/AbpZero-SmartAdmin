namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResourcesDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "Description");
        }
    }
}
