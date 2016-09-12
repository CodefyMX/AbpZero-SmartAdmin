namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbUpdateIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Url");
        }
    }
}
