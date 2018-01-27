namespace SmartSSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitralCreate6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UploadFiles", "FileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UploadFiles", "FileType", c => c.String(unicode: false));
        }
    }
}
