namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreviewImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "PreviewImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "PreviewImage");
        }
    }
}
