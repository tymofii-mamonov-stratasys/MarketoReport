namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeadsAndActivitiesPKfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeadActivities", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.LeadActivities", "LeadId", "dbo.Leads");
            DropPrimaryKey("dbo.Activities");
            DropPrimaryKey("dbo.Leads");
            AddColumn("dbo.Activities", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Leads", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Activities", "ActivityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Leads", "LeadId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Activities", "Id");
            AddPrimaryKey("dbo.Leads", "Id");
            AddForeignKey("dbo.LeadActivities", "ActivityId", "dbo.Activities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeadActivities", "LeadId", "dbo.Leads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeadActivities", "LeadId", "dbo.Leads");
            DropForeignKey("dbo.LeadActivities", "ActivityId", "dbo.Activities");
            DropPrimaryKey("dbo.Leads");
            DropPrimaryKey("dbo.Activities");
            AlterColumn("dbo.Leads", "LeadId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Activities", "ActivityId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Leads", "Id");
            DropColumn("dbo.Activities", "Id");
            AddPrimaryKey("dbo.Leads", "LeadId");
            AddPrimaryKey("dbo.Activities", "ActivityId");
            AddForeignKey("dbo.LeadActivities", "LeadId", "dbo.Leads", "LeadId", cascadeDelete: true);
            AddForeignKey("dbo.LeadActivities", "ActivityId", "dbo.Activities", "ActivityId", cascadeDelete: true);
        }
    }
}
