namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeCampaignIdFieldNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leads", "CampaignId", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leads", "CampaignId", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
