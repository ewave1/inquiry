namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(unicode: false),
                        ContactName = c.String(unicode: false),
                        ContactMobile = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        Remark = c.String(unicode: false),
                        CreateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialGravities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        Hardness = c.Int(nullable: false),
                        Gravity = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialHoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        SizeC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HoleCount = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        SizeB = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Hours = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        SizeB = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BadRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SizeB = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Factory = c.String(unicode: false),
                        Material = c.String(unicode: false),
                        MaterialId = c.Int(nullable: false),
                        Material1 = c.String(unicode: false),
                        Material2 = c.String(unicode: false),
                        Color = c.String(unicode: false),
                        Hardness = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UploadFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileUrl = c.String(unicode: false),
                        FileName = c.String(unicode: false),
                        LocalPath = c.String(unicode: false),
                        FileType = c.String(unicode: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.InquiryLogs", "Color", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "Hardness", c => c.Int(nullable: false));
            AddColumn("dbo.Materials", "Display", c => c.String(unicode: false));
            AddColumn("dbo.Materials", "SpecialDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Materials", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Materials", "UpdateUser", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Materials", "UpdateUser");
            DropColumn("dbo.Materials", "UpdateTime");
            DropColumn("dbo.Materials", "SpecialDiscount");
            DropColumn("dbo.Materials", "Display");
            DropColumn("dbo.InquiryLogs", "Hardness");
            DropColumn("dbo.InquiryLogs", "Color");
            DropTable("dbo.UploadFiles");
            DropTable("dbo.Storages");
            DropTable("dbo.MaterialRates");
            DropTable("dbo.MaterialHours");
            DropTable("dbo.MaterialHoles");
            DropTable("dbo.MaterialGravities");
            DropTable("dbo.MaterialFeatures");
            DropTable("dbo.Customers");
        }
    }
}
