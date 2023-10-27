namespace EZFormsPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flagLevelNameChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flag",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 99),
                        Message = c.String(),
                        TriggerExpression = c.String(),
                        Level = c.String(),
                        FieldID = c.Int(nullable: false),
                        FormID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Flag");
        }
    }
}
