namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Field", "FormOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Field", "FormOrder");
        }
    }
}
