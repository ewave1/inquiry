namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InquiryLogs", "Material1", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "Material2", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InquiryLogs", "Material2");
            DropColumn("dbo.InquiryLogs", "Material1");
        }
    }
}
