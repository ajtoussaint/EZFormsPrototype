namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Field",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FormID = c.Int(nullable: false),
                        Name = c.String(),
                        Type = c.String(),
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
