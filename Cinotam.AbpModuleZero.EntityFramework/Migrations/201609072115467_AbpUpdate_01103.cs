namespace Cinotam.AbpModuleZero.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AbpUpdate_01103 : DbMigration
    {
        //public override void Up()
        //{
        //    AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 95));
        //}

        //public override void Down()
        //{
        //    AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 128));
        //}

        //Fix https://github.com/aspnetboilerplate/module-zero/releases
        public override void Up()
        {
            DropIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
            AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 95));
            CreateIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
        }

        public override void Down()
        {
            DropIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
            AlterColumn("dbo.AbpOrganizationUnits", "Code", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AbpOrganizationUnits", new[] { "TenantId", "Code" });
        }
    }
}

