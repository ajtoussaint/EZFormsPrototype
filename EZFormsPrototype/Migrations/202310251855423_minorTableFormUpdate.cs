namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minorTableFormUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Field", "FormID", "dbo.Form");
            DropIndex("dbo.Field", new[] { "FormID" });
            AddColumn("dbo.Field", "Form_ID", c => c.Int());
            CreateIndex("dbo.Field", "Form_ID");
            AddForeignKey("dbo.Field", "Form_ID", "dbo.Form", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Field", "Form_ID", "dbo.Form");
            DropIndex("dbo.Field", new[] { "Form_ID" });
            DropColumn("dbo.Field", "Form_ID");
            CreateIndex("dbo.Field", "FormID");
            AddForeignKey("dbo.Field", "FormID", "dbo.Form", "ID", cascadeDelete: true);
        }
    }
}
