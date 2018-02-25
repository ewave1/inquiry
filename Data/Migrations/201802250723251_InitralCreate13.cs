namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Storages", "Name", c => c.String(unicode: false));
            AddColumn("dbo.Storages", "Spec", c => c.String(unicode: false));
            AddColumn("dbo.Storages", "BatchNo", c => c.String(unicode: false));
            AddColumn("dbo.Storages", "Location", c => c.String(unicode: false));
            AddColumn("dbo.Storages", "Remark", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Storages", "Remark");
            DropColumn("dbo.Storages", "Location");
            DropColumn("dbo.Storages", "BatchNo");
            DropColumn("dbo.Storages", "Spec");
            DropColumn("dbo.Storages", "Name");
        }
    }
}
