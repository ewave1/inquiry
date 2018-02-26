namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MaterialStartAmounts", "SizeC2", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MaterialStartAmounts", "SizeC2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
