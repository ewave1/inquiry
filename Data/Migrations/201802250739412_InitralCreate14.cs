namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Storages", "MaterialDisplay", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Storages", "MaterialDisplay");
        }
    }
}
