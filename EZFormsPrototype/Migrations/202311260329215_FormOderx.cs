namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormOderx : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Field", "FormOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Field", "FormOrder", c => c.Int(nullable: false));
        }
    }
}
