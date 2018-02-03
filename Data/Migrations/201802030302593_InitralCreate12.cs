namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InquiryLogs", "CustomerLevel", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "Storage", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.Storages", "MaterialCode", c => c.String(unicode: false));
            DropColumn("dbo.InquiryLogs", "Material");
            DropColumn("dbo.Storages", "Material");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Storages", "Material", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "Material", c => c.String(unicode: false));
            DropColumn("dbo.Storages", "MaterialCode");
            DropColumn("dbo.InquiryLogs", "MaterialCode");
            DropColumn("dbo.InquiryLogs", "Storage");
            DropColumn("dbo.InquiryLogs", "CustomerLevel");
        }
    }
}
