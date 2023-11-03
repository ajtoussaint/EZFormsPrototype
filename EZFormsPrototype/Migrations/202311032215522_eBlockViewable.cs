namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eBlockViewable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpressionBlock", "CodeExpression", c => c.String());
            AddColumn("dbo.ExpressionBlock", "ViewExpression", c => c.String());
            DropColumn("dbo.ExpressionBlock", "Expression");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExpressionBlock", "Expression", c => c.String());
            DropColumn("dbo.ExpressionBlock", "ViewExpression");
            DropColumn("dbo.ExpressionBlock", "CodeExpression");
        }
    }
}
