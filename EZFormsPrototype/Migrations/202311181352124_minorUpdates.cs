namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minorUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Field", "Field_ID", c => c.Int());
            CreateIndex("dbo.Field", "Field_ID");
            AddForeignKey("dbo.Field", "Field_ID", "dbo.Field", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Field", "Field_ID", "dbo.Field");
            DropIndex("dbo.Field", new[] { "Field_ID" });
            DropColumn("dbo.Field", "Field_ID");
        }
    }
}
