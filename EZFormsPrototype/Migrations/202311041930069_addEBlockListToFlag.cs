namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEBlockListToFlag : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ExpressionBlock", "FlagID");
            AddForeignKey("dbo.ExpressionBlock", "FlagID", "dbo.Flag", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpressionBlock", "FlagID", "dbo.Flag");
            DropIndex("dbo.ExpressionBlock", new[] { "FlagID" });
        }
    }
}
