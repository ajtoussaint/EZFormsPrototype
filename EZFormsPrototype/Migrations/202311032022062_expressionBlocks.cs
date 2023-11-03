namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expressionBlocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpressionBlock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        DependantFieldID1 = c.Int(nullable: false),
                        DependantFieldID2 = c.Int(nullable: false),
                        Expression = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExpressionBlock");
        }
    }
}
