namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BaseHoles", "HoleCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BaseHoles", "HoleCount", c => c.Int(nullable: false));
        }
    }
}
