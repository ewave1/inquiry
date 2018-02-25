namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialHoles", "SizeC2", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.MaterialHoles", "HoleCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaterialHoles", "HoleCount", c => c.Int(nullable: false));
            DropColumn("dbo.MaterialHoles", "SizeC2");
        }
    }
}
