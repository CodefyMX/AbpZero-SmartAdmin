namespace Cinotam.AbpModuleZero.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CmsRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryContents", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Pages", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Pages", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.Chunks", "ContentId", "dbo.Contents");
            DropForeignKey("dbo.Contents", "PageId", "dbo.Pages");
            DropForeignKey("dbo.MenuContents", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.MenuSections", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.MenuSections", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.MenuSectionContents", "SectionId", "dbo.MenuSections");
            DropForeignKey("dbo.MenuSectionItems", "SectionId", "dbo.MenuSections");
            DropForeignKey("dbo.MenuSectionItemContents", "SectionItemId", "dbo.MenuSectionItems");
            DropForeignKey("dbo.MenuSectionItemContents", "PageId", "dbo.Pages");
            DropForeignKey("dbo.MenuSectionItems", "MenuSectionContent_Id", "dbo.MenuSectionContents");
            DropForeignKey("dbo.Resources", "TemplateId", "dbo.Templates");
            DropIndex("dbo.CategoryContents", new[] { "CategoryId" });
            DropIndex("dbo.Pages", new[] { "CategoryId" });
            DropIndex("dbo.Pages", new[] { "Page_Id" });
            DropIndex("dbo.Contents", new[] { "PageId" });
            DropIndex("dbo.Chunks", new[] { "ContentId" });
            DropIndex("dbo.MenuContents", new[] { "MenuId" });
            DropIndex("dbo.MenuSections", new[] { "MenuId" });
            DropIndex("dbo.MenuSections", new[] { "CategoryId" });
            DropIndex("dbo.MenuSectionContents", new[] { "SectionId" });
            DropIndex("dbo.MenuSectionItems", new[] { "SectionId" });
            DropIndex("dbo.MenuSectionItems", new[] { "MenuSectionContent_Id" });
            DropIndex("dbo.MenuSectionItemContents", new[] { "SectionItemId" });
            DropIndex("dbo.MenuSectionItemContents", new[] { "PageId" });
            DropIndex("dbo.Resources", new[] { "TemplateId" });
            DropTable("dbo.Categories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Category_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Category_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CategoryContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CategoryContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_CategoryContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Pages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Page_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Page_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Contents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Content_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Chunks",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Chunk_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Menus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Menu_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Menu_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSections",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSection_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSection_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSectionContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSectionItems",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionItem_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuSectionItemContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuSectionItemContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionItemContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Resources",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Resource_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Resource_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Templates",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Template_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Template_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileName = c.String(),
                        Content = c.String(),
                        IsPartial = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Template_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Template_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceType = c.String(),
                        ResourceUrl = c.String(),
                        Description = c.String(),
                        IsCdn = c.Boolean(nullable: false),
                        TemplateId = c.Int(nullable: false),
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
                    { "DynamicFilter_Resource_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Resource_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSectionItemContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayText = c.String(),
                        SectionItemId = c.Int(nullable: false),
                        PageId = c.Int(),
                        Url = c.String(),
                        Lang = c.String(),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSectionItemContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionItemContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SectionId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PageId = c.Int(),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSectionItem_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSectionContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(),
                        DisplayText = c.String(),
                        SectionId = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSectionContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSectionContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionName = c.String(),
                        MenuId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuSection_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuSection_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuName = c.String(),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_Menu_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Menu_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Lang = c.String(),
                        Url = c.String(),
                        MenuId = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_MenuContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MenuContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chunks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Order = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
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
                    { "DynamicFilter_Chunk_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(),
                        PageId = c.Int(nullable: false),
                        HtmlContent = c.String(),
                        Title = c.String(),
                        TemplateUniqueName = c.String(),
                        IsPartial = c.Boolean(nullable: false),
                        Url = c.String(),
                        PreviewImage = c.String(),
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
                    { "DynamicFilter_Content_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentPage = c.Int(),
                        Active = c.Boolean(nullable: false),
                        TemplateName = c.String(),
                        IsMainPage = c.Boolean(nullable: false),
                        CategoryId = c.Int(),
                        IncludeInMenu = c.Boolean(nullable: false),
                        ShowBreadCrum = c.Boolean(nullable: false),
                        BreadCrumInContainer = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Page_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Page_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Page_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(),
                        DisplayText = c.String(),
                        CategoryId = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_CategoryContent_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_CategoryContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        TenantId = c.Int(nullable: false),
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
                    { "DynamicFilter_Category_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Category_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Resources", "TemplateId");
            CreateIndex("dbo.MenuSectionItemContents", "PageId");
            CreateIndex("dbo.MenuSectionItemContents", "SectionItemId");
            CreateIndex("dbo.MenuSectionItems", "MenuSectionContent_Id");
            CreateIndex("dbo.MenuSectionItems", "SectionId");
            CreateIndex("dbo.MenuSectionContents", "SectionId");
            CreateIndex("dbo.MenuSections", "CategoryId");
            CreateIndex("dbo.MenuSections", "MenuId");
            CreateIndex("dbo.MenuContents", "MenuId");
            CreateIndex("dbo.Chunks", "ContentId");
            CreateIndex("dbo.Contents", "PageId");
            CreateIndex("dbo.Pages", "Page_Id");
            CreateIndex("dbo.Pages", "CategoryId");
            CreateIndex("dbo.CategoryContents", "CategoryId");
            AddForeignKey("dbo.Resources", "TemplateId", "dbo.Templates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuSectionItems", "MenuSectionContent_Id", "dbo.MenuSectionContents", "Id");
            AddForeignKey("dbo.MenuSectionItemContents", "PageId", "dbo.Pages", "Id");
            AddForeignKey("dbo.MenuSectionItemContents", "SectionItemId", "dbo.MenuSectionItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuSectionItems", "SectionId", "dbo.MenuSections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuSectionContents", "SectionId", "dbo.MenuSections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuSections", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MenuSections", "CategoryId", "dbo.Categories", "Id");
            AddForeignKey("dbo.MenuContents", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Contents", "PageId", "dbo.Pages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Chunks", "ContentId", "dbo.Contents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Pages", "Page_Id", "dbo.Pages", "Id");
            AddForeignKey("dbo.Pages", "CategoryId", "dbo.Categories", "Id");
            AddForeignKey("dbo.CategoryContents", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
