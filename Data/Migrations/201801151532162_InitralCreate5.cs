namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "UpdateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.Customers", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.Customers", "CustomerLevel", c => c.String(unicode: false));
            AlterColumn("dbo.Customers", "CreateUser", c => c.String(unicode: false));
            AlterColumn("dbo.Materials", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialFeatures", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialGravities", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialHoles", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialHours", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.MaterialRates", "UpdateUser", c => c.String(unicode: false));
            AlterColumn("dbo.Storages", "UpdateUser", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Storages", "UpdateUser", c => c.Int());
            AlterColumn("dbo.MaterialRates", "UpdateUser", c => c.Int());
            AlterColumn("dbo.MaterialHours", "UpdateUser", c => c.Int());
            AlterColumn("dbo.MaterialHoles", "UpdateUser", c => c.Int());
            AlterColumn("dbo.MaterialGravities", "UpdateUser", c => c.Int());
            AlterColumn("dbo.MaterialFeatures", "UpdateUser", c => c.Int());
            AlterColumn("dbo.Materials", "UpdateUser", c => c.Int());
            AlterColumn("dbo.Customers", "CreateUser", c => c.Int());
            AlterColumn("dbo.Customers", "CustomerLevel", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "UpdateUser");
            DropColumn("dbo.Customers", "UpdateTime");
        }
    }
}
