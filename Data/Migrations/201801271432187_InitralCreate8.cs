namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PT_ImportHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportTime = c.DateTime(nullable: false, precision: 0),
                        ImportType = c.Int(nullable: false),
                        User = c.String(unicode: false),
                        TotalCount = c.Int(nullable: false),
                        SuccessCount = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ImportFileName = c.String(unicode: false),
                        ImportFile = c.String(unicode: false),
                        ReturnFile = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PT_ImportHistoryDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportID = c.Int(nullable: false),
                        RelateID = c.Int(),
                        IsSuccess = c.Int(nullable: false),
                        Json = c.String(unicode: false),
                        ErrorInfo = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PT_ImportHistoryDetail");
            DropTable("dbo.PT_ImportHistory");
        }
    }
}
