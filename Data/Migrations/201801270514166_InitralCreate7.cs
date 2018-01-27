namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UploadFiles", "UpdateUser", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UploadFiles", "UpdateUser", c => c.Int(nullable: false));
        }
    }
}
