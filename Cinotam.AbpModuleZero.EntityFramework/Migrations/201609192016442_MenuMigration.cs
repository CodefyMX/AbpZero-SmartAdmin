namespace Cinotam.AbpModuleZero.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class MenuMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuContents",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Text = c.String(),
                    Lang = c.String(),
                    Url = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    Menu_Id = c.Int(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Menu_Id);

            CreateTable(
                "dbo.Menus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MenuName = c.String(),
                    ParentId = c.Int(),
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
                    { "DynamicFilter_Menu_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.Page_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Menus", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.MenuContents", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.Menus", new[] { "Page_Id" });
            DropIndex("dbo.MenuContents", new[] { "Menu_Id" });
            DropTable("dbo.Menus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Menu_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MenuContents",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MenuContent_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });

        }
    }
}
