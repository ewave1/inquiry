namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CustomerLevel", c => c.Int(nullable: false));
            DropColumn("dbo.Storages", "Factory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Storages", "Factory", c => c.String(unicode: false));
            DropColumn("dbo.Customers", "CustomerLevel");
        }
    }
}
