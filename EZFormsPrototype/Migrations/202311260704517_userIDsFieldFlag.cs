namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIDsFieldFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Field", "userID", c => c.String());
            AddColumn("dbo.Flag", "userID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Flag", "userID");
            DropColumn("dbo.Field", "userID");
        }
    }
}
