namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class regexNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Field", "Name", c => c.String(maxLength: 99));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Field", "Name", c => c.String());
        }
    }
}
