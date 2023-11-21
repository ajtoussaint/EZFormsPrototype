namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tidyOnSubmit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExpressionBlock", "FlagID", "dbo.Flag");
            DropIndex("dbo.ExpressionBlock", new[] { "FlagID" });
            RenameColumn(table: "dbo.Field", name: "Field_ID", newName: "FormField_ID");
            RenameIndex(table: "dbo.Field", name: "IX_Field_ID", newName: "IX_FormField_ID");
            AddColumn("dbo.Flag", "appearsOnSubmit", c => c.Boolean(nullable: false));
            DropColumn("dbo.Flag", "TriggerExpression");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flag", "TriggerExpression", c => c.String());
            DropColumn("dbo.Flag", "appearsOnSubmit");
            RenameIndex(table: "dbo.Field", name: "IX_FormField_ID", newName: "IX_Field_ID");
            RenameColumn(table: "dbo.Field", name: "FormField_ID", newName: "Field_ID");
            CreateIndex("dbo.ExpressionBlock", "FlagID");
            AddForeignKey("dbo.ExpressionBlock", "FlagID", "dbo.Flag", "ID", cascadeDelete: true);
        }
    }
}
