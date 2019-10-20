namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate22 : DbMigration
    {
        public override void Up()
        { 
            DropPrimaryKey("DiscountSets");
          
                
            
            //AddColumn("dbo.Customers", "Id", c => c.Int(nullable: false, identity: true));
            //AddColumn("dbo.Customers", "CompanyName", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "ContactName", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "ContactMobile", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "CustomerLevel", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "Remark", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "CreateUser", c => c.String(unicode: false));
            //AddColumn("dbo.Customers", "UpdateTime", c => c.DateTime(precision: 0));
            //AddColumn("dbo.Customers", "UpdateUser", c => c.String(unicode: false));
            //AddColumn("dbo.DiscountSets", "Id", c => c.Int(nullable: false, identity: true));
            //AddColumn("dbo.InquiryLogs", "CustomerLevel", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "Storage", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "MaterialCode", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "Material1", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "Material2", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "Color", c => c.String(unicode: false));
            //AddColumn("dbo.InquiryLogs", "Hardness", c => c.Int(nullable: false));
            //AddColumn("dbo.Materials", "MaterialCode", c => c.String(unicode: false));
            //AddColumn("dbo.Materials", "Code", c => c.String(unicode: false));
            //AddColumn("dbo.Materials", "Hardness", c => c.Int(nullable: false));
            //AddColumn("dbo.Materials", "Display", c => c.String(unicode: false));
            //AddColumn("dbo.Materials", "SpecialDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AddColumn("dbo.Materials", "IsDefault", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Materials", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AddColumn("dbo.Materials", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            //AddColumn("dbo.Materials", "UpdateUser", c => c.String(unicode: false));
            //AlterColumn("dbo.DiscountSets", "Name", c => c.String(maxLength: 50, storeType: "nvarchar"));
            //AddPrimaryKey("dbo.Customers", "Id");
            //AddPrimaryKey("dbo.DiscountSets", "Id");
            //DropColumn("dbo.Customers", "CustomerNo");
            //DropColumn("dbo.Customers", "CustomerName");
            //DropColumn("dbo.Customers", "Mobile");
            //DropColumn("dbo.Customers", "Description");
            //DropColumn("dbo.Customers", "BelongToUser");
            //DropColumn("dbo.InquiryLogs", "Material");
            //DropColumn("dbo.Materials", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Materials", "Name", c => c.String(unicode: false));
            AddColumn("dbo.InquiryLogs", "Material", c => c.String(unicode: false));
            AddColumn("dbo.Customers", "BelongToUser", c => c.String(maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Customers", "Description", c => c.String(unicode: false));
            AddColumn("dbo.Customers", "Mobile", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            AddColumn("dbo.Customers", "CustomerName", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Customers", "CustomerNo", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            DropPrimaryKey("dbo.DiscountSets");
            DropPrimaryKey("dbo.Customers");
            AlterColumn("dbo.DiscountSets", "Name", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            DropColumn("dbo.Materials", "UpdateUser");
            DropColumn("dbo.Materials", "UpdateTime");
            DropColumn("dbo.Materials", "Price");
            DropColumn("dbo.Materials", "IsDefault");
            DropColumn("dbo.Materials", "SpecialDiscount");
            DropColumn("dbo.Materials", "Display");
            DropColumn("dbo.Materials", "Hardness");
            DropColumn("dbo.Materials", "Code");
            DropColumn("dbo.Materials", "MaterialCode");
            DropColumn("dbo.InquiryLogs", "Hardness");
            DropColumn("dbo.InquiryLogs", "Color");
            DropColumn("dbo.InquiryLogs", "Material2");
            DropColumn("dbo.InquiryLogs", "Material1");
            DropColumn("dbo.InquiryLogs", "MaterialCode");
            DropColumn("dbo.InquiryLogs", "Storage");
            DropColumn("dbo.InquiryLogs", "CustomerLevel");
            DropColumn("dbo.DiscountSets", "Id");
            DropColumn("dbo.Customers", "UpdateUser");
            DropColumn("dbo.Customers", "UpdateTime");
            DropColumn("dbo.Customers", "CreateUser");
            DropColumn("dbo.Customers", "Remark");
            DropColumn("dbo.Customers", "CustomerLevel");
            DropColumn("dbo.Customers", "ContactMobile");
            DropColumn("dbo.Customers", "ContactName");
            DropColumn("dbo.Customers", "CompanyName");
            DropColumn("dbo.Customers", "Id");
            DropTable("dbo.UploadFiles");
            DropTable("dbo.Storages");
            DropTable("dbo.StandardSizes");
            DropTable("dbo.PT_ImportHistoryDetail");
            DropTable("dbo.PT_ImportHistory");
            DropTable("dbo.MaterialStorages");
            DropTable("dbo.MaterialStartAmounts");
            DropTable("dbo.MaterialRates");
            DropTable("dbo.MaterialHours");
            DropTable("dbo.MaterialHoles");
            DropTable("dbo.MaterialGravities");
            DropTable("dbo.MaterialFeatures");
            DropTable("dbo.BaseHoles");
            AddPrimaryKey("dbo.DiscountSets", "Name");
            AddPrimaryKey("dbo.Customers", "CustomerNo");
        }
    }
}
