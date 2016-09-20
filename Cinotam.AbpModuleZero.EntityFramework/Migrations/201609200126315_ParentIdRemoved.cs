namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentIdRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Menus", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "ParentId", c => c.Int());
        }
    }
}
