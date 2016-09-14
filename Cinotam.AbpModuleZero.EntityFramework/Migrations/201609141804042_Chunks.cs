namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Chunks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chunks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Content_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contents", t => t.Content_Id)
                .Index(t => t.Content_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chunks", "Content_Id", "dbo.Contents");
            DropIndex("dbo.Chunks", new[] { "Content_Id" });
            DropTable("dbo.Chunks",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
