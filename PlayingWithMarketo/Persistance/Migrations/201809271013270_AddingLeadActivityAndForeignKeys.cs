namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLeadActivityAndForeignKeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeadActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.Int(nullable: false),
                        ActivityDate = c.DateTime(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        Attributes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Leads", t => t.LeadId, cascadeDelete: true)
                .Index(t => t.LeadId)
                .Index(t => t.ActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeadActivities", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.LeadActivities", "ActivityId", "dbo.Activities");
            DropIndex("dbo.LeadActivities", new[] { "ActivityId" });
            DropIndex("dbo.LeadActivities", new[] { "LeadId" });
            DropTable("dbo.LeadActivities");
        }
    }
}
