namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizableContentDtoProp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpCinotamLocalizableContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityId = c.String(),
                        EntityName = c.String(),
                        EntityDtoName = c.String(),
                        Properties = c.String(),
                        Lang = c.String(),
                        TenantId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AbpCinotamLocalizableContent_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AbpCinotamLocalizableContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AbpCinotamLocalizableContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AbpCinotamLocalizableContent_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AbpCinotamLocalizableContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
