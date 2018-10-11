namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeadsAndAcitivities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acitivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        ActivityName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeadId = c.Int(nullable: false),
                        CampaignId = c.String(nullable: false, maxLength: 25),
                        SFDCId = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leads");
            DropTable("dbo.Acitivities");
        }
    }
}
