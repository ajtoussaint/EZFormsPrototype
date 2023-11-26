namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIDForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Form", "userID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Form", "userID");
        }
    }
}
