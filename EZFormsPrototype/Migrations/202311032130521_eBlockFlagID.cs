namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eBlockFlagID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpressionBlock", "FlagID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExpressionBlock", "FlagID");
        }
    }
}
