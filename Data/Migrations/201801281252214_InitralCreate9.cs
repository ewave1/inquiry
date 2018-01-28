namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseHoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeC = c.Int(nullable: false),
                        HoleCount = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUser = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Materials", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.Materials", "Hardness", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialFeatures", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.MaterialFeatures", "Hardness", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialFeatures", "Name", c => c.String(unicode: false));
            AddColumn("dbo.MaterialGravities", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.MaterialGravities", "Color", c => c.String(unicode: false));
            AddColumn("dbo.MaterialHoles", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.MaterialHoles", "Hardness", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialHoles", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MaterialHours", "MaterialCode", c => c.String(unicode: false));
            AddColumn("dbo.MaterialHours", "Hardness", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialHours", "SizeB2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MaterialHours", "MosInHour", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialRates", "SizeB2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MaterialGravities", "Gravity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Materials", "Name");
            DropColumn("dbo.MaterialHours", "Hours");
            DropColumn("dbo.MaterialRates", "MaterialId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaterialRates", "MaterialId", c => c.Int(nullable: false));
            AddColumn("dbo.MaterialHours", "Hours", c => c.Int(nullable: false));
            AddColumn("dbo.Materials", "Name", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialGravities", "Gravity", c => c.Int(nullable: false));
            DropColumn("dbo.MaterialRates", "SizeB2");
            DropColumn("dbo.MaterialHours", "MosInHour");
            DropColumn("dbo.MaterialHours", "SizeB2");
            DropColumn("dbo.MaterialHours", "Hardness");
            DropColumn("dbo.MaterialHours", "MaterialCode");
            DropColumn("dbo.MaterialHoles", "Rate");
            DropColumn("dbo.MaterialHoles", "Hardness");
            DropColumn("dbo.MaterialHoles", "MaterialCode");
            DropColumn("dbo.MaterialGravities", "Color");
            DropColumn("dbo.MaterialGravities", "MaterialCode");
            DropColumn("dbo.MaterialFeatures", "Name");
            DropColumn("dbo.MaterialFeatures", "Hardness");
            DropColumn("dbo.MaterialFeatures", "MaterialCode");
            DropColumn("dbo.Materials", "Hardness");
            DropColumn("dbo.Materials", "MaterialCode");
            DropTable("dbo.BaseHoles");
        }
    }
}
