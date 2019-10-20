namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StandardSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SizeB = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(unicode: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StandardSizes");
        }
    }
}
