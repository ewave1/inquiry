namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppInfoes",
                c => new
                    {
                        AppKey = c.String(nullable: false, maxLength: 32),
                        AppSecret = c.String(nullable: false, maxLength: 32),
                        Title = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(nullable: false, maxLength: 200),
                        Icon = c.String(nullable: false, maxLength: 1024),
                        ReturnUrl = c.String(nullable: false, maxLength: 1024),
                        IsEnable = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AppKey);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserPwd = c.String(nullable: false, maxLength: 32),
                        Nick = c.String(nullable: false, maxLength: 50),
                        IsEnable = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.ManageUsers",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserPwd = c.String(nullable: false, maxLength: 32),
                        Nick = c.String(nullable: false, maxLength: 50),
                        IsEnable = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.UserAuthOperates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SessionKey = c.String(nullable: false, maxLength: 32),
                        Remark = c.String(nullable: false, maxLength: 1024),
                        IpAddress = c.String(nullable: false, maxLength: 15),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserAuthSessions",
                c => new
                    {
                        SessionKey = c.String(nullable: false, maxLength: 32),
                        AppKey = c.String(nullable: false, maxLength: 32),
                        UserName = c.String(maxLength: 50),
                        IpAddress = c.String(nullable: false, maxLength: 15),
                        InvalidTime = c.DateTime(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SessionKey);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserAuthSessions");
            DropTable("dbo.UserAuthOperates");
            DropTable("dbo.ManageUsers");
            DropTable("dbo.AppUsers");
            DropTable("dbo.AppInfoes");
        }
    }
}
