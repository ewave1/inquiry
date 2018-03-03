namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaterialStorages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SizeB = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaterialCode = c.String(unicode: false),
                        Spec = c.String(unicode: false),
                        Spec2 = c.String(unicode: false),
                        BatchNo = c.String(unicode: false),
                        Location = c.String(unicode: false),
                        Remark = c.String(unicode: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Materials", "Code", c => c.String(unicode: false));
            AddColumn("dbo.MaterialFeatures", "Code", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaterialFeatures", "Code");
            DropColumn("dbo.Materials", "Code");
            DropTable("dbo.MaterialStorages");
        }
    }
}
