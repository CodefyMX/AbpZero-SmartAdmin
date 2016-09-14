namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderForChunks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chunks", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chunks", "Order");
        }
    }
}
