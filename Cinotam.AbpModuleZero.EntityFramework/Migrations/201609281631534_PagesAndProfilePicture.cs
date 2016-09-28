namespace Cinotam.AbpModuleZero.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class PagesAndProfilePicture : DbMigration
    {
        //See https://github.com/aspnetboilerplate/module-zero/releases/tag/v0.11.2.0
        public override void Up()
        {
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
                    { "DynamicFilter_Page_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.Page_Id);

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    DisplayName = c.String(),
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
                    { "DynamicFilter_Category_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
                    { "DynamicFilter_CategoryContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);

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
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pages", t => t.PageId, cascadeDelete: true)
                .Index(t => t.PageId);

            CreateTable(
                "dbo.Chunks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Key = c.String(),
                    Value = c.String(),
                    Order = c.Int(nullable: false),
                    ContentId = c.Int(nullable: false),
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
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contents", t => t.ContentId, cascadeDelete: true)
                .Index(t => t.ContentId);

            CreateTable(
                "dbo.AbpUserClaims",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TenantId = c.Int(),
                    UserId = c.Long(nullable: false),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            AddColumn("dbo.AbpUsers", "ProfilePicture", c => c.String());
            AddColumn("dbo.AbpUsers", "IsPictureOnCdn", c => c.Boolean(nullable: false));
            AddColumn("dbo.AbpUsers", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.AbpUsers", "AccessFailedCount", c => c.Int(nullable: false));
            AddColumn("dbo.AbpUsers", "IsLockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AbpUsers", "PhoneNumber", c => c.String());
            AddColumn("dbo.AbpUsers", "IsPhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AbpUsers", "SecurityStamp", c => c.String());
            AddColumn("dbo.AbpUsers", "IsTwoFactorEnabled", c => c.Boolean(nullable: false));
            DropIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
            AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 95));
            CreateIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
        }

        public override void Down()
        {
            DropForeignKey("dbo.AbpUserClaims", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.Contents", "PageId", "dbo.Pages");
            DropForeignKey("dbo.Chunks", "ContentId", "dbo.Contents");
            DropForeignKey("dbo.Pages", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.Pages", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryContents", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AbpUserClaims", new[] { "UserId" });
            DropIndex("dbo.Chunks", new[] { "ContentId" });
            DropIndex("dbo.Contents", new[] { "PageId" });
            DropIndex("dbo.CategoryContents", new[] { "CategoryId" });
            DropIndex("dbo.Pages", new[] { "Page_Id" });
            DropIndex("dbo.Pages", new[] { "CategoryId" });
            DropIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
            AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
            DropColumn("dbo.AbpUsers", "IsTwoFactorEnabled");
            DropColumn("dbo.AbpUsers", "SecurityStamp");
            DropColumn("dbo.AbpUsers", "IsPhoneNumberConfirmed");
            DropColumn("dbo.AbpUsers", "PhoneNumber");
            DropColumn("dbo.AbpUsers", "IsLockoutEnabled");
            DropColumn("dbo.AbpUsers", "AccessFailedCount");
            DropColumn("dbo.AbpUsers", "LockoutEndDateUtc");
            DropColumn("dbo.AbpUsers", "IsPictureOnCdn");
            DropColumn("dbo.AbpUsers", "ProfilePicture");
            DropTable("dbo.AbpUserClaims",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Chunks",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Chunk_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Contents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CategoryContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CategoryContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Categories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Category_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Pages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Page_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
