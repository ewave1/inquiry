namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaterialStartAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SizeC2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StorageType = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MaterialStartAmounts");
        }
    }
}
