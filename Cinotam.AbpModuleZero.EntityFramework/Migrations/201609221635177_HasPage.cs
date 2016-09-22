namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuSectionItemContents", "PageId", c => c.Int());
            AddColumn("dbo.MenuSectionItemContents", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuSectionItemContents", "Url");
            DropColumn("dbo.MenuSectionItemContents", "PageId");
        }
    }
}
