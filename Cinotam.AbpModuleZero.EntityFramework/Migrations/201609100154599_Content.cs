namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "Content");
        }
    }
}
