namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Field", "TableID", c => c.Int());
            AddColumn("dbo.Field", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Field", "Discriminator");
            DropColumn("dbo.Field", "TableID");
        }
    }
}
