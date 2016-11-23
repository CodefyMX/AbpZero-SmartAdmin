namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Properties", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Properties");
        }
    }
}
