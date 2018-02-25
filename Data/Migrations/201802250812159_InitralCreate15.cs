namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Materials", "IsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.MaterialFeatures", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaterialFeatures", "IsDefault");
            DropColumn("dbo.Materials", "IsDefault");
        }
    }
}
