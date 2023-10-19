namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormItemReset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Field",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FormID = c.Int(nullable: false),
                        FormOrder = c.Int(nullable: false),
                        Type = c.String(),
                        TableID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Form", t => t.FormID, cascadeDelete: true)
                .Index(t => t.FormID);
            
            CreateTable(
                "dbo.Form",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Field", "FormID", "dbo.Form");
            DropIndex("dbo.Field", new[] { "FormID" });
            DropTable("dbo.Form");
            DropTable("dbo.Field");
        }
    }
}
