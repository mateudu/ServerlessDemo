namespace ServerlessDemo.Database.Migrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageAndComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        OwnerUpn = c.String(),
                        AddedAt = c.DateTime(nullable: false),
                        Text = c.String(),
                        Allowed = c.Boolean(),
                        ImageId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.ImageId, name: "IX_Image_Id");
            
            AddColumn("dbo.Images", "RelativePath", c => c.String());
            AddColumn("dbo.Images", "ThumbnailRelativePath", c => c.String());
            AddColumn("dbo.Images", "OwnerUpn", c => c.String());
            AddColumn("dbo.Images", "AddedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "Uploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.Images", "Allowed", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ImageId", "dbo.Images");
            DropIndex("dbo.Comments", "IX_Image_Id");
            DropColumn("dbo.Images", "Allowed");
            DropColumn("dbo.Images", "Uploaded");
            DropColumn("dbo.Images", "AddedAt");
            DropColumn("dbo.Images", "OwnerUpn");
            DropColumn("dbo.Images", "ThumbnailRelativePath");
            DropColumn("dbo.Images", "RelativePath");
            DropTable("dbo.Comments");
        }
    }
}
