namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeValidationCampaignId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leads", "CampaignId", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leads", "CampaignId", c => c.String(maxLength: 25));
        }
    }
}
