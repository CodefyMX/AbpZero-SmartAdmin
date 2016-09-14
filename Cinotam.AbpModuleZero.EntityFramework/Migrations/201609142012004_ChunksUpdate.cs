namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChunksUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Chunks", name: "Content_Id", newName: "ContentObj_Id");
            RenameIndex(table: "dbo.Chunks", name: "IX_Content_Id", newName: "IX_ContentObj_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Chunks", name: "IX_ContentObj_Id", newName: "IX_Content_Id");
            RenameColumn(table: "dbo.Chunks", name: "ContentObj_Id", newName: "Content_Id");
        }
    }
}
