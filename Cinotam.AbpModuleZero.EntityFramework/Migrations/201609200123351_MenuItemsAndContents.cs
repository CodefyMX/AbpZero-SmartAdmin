namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class MenuItemsAndContents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionName = c.String(),
                        MenuId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSection_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.MenuSectionContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(),
                        DisplayText = c.String(),
                        SectionId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSectionContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuSections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.MenuSectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SectionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        MenuSectionContent_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuSections", t => t.SectionId, cascadeDelete: true)
                .ForeignKey("dbo.MenuSectionContents", t => t.MenuSectionContent_Id)
                .Index(t => t.SectionId)
                .Index(t => t.MenuSectionContent_Id);
            
            CreateTable(
                "dbo.MenuSectionItemContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayText = c.String(),
                        SectionItemId = c.Int(nullable: false),
                        Lang = c.String(),
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
                    { "DynamicFilter_MenuSectionItemContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuSectionItems", t => t.SectionItemId, cascadeDelete: true)
                .Index(t => t.SectionItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuSectionItems", "MenuSectionContent_Id", "dbo.MenuSectionContents");
            DropForeignKey("dbo.MenuSectionItemContents", "SectionItemId", "dbo.MenuSectionItems");
            DropForeignKey("dbo.MenuSectionItems", "SectionId", "dbo.MenuSections");
            DropForeignKey("dbo.MenuSectionContents", "SectionId", "dbo.MenuSections");
            DropForeignKey("dbo.MenuSections", "MenuId", "dbo.Menus");
            DropIndex("dbo.MenuSectionItemContents", new[] { "SectionItemId" });
            DropIndex("dbo.MenuSectionItems", new[] { "MenuSectionContent_Id" });
            DropIndex("dbo.MenuSectionItems", new[] { "SectionId" });
            DropIndex("dbo.MenuSectionContents", new[] { "SectionId" });
            DropIndex("dbo.MenuSections", new[] { "MenuId" });
            DropTable("dbo.MenuSectionItemContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionItemContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSectionItems",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSectionContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSections",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSection_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
