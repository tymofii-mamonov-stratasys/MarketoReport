namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingPKs : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Activities");
            DropPrimaryKey("dbo.Leads");
            AlterColumn("dbo.Activities", "ActivityId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Leads", "LeadId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Activities", "ActivityId");
            AddPrimaryKey("dbo.Leads", "LeadId");
            DropColumn("dbo.Activities", "Id");
            DropColumn("dbo.Leads", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leads", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Activities", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Leads");
            DropPrimaryKey("dbo.Activities");
            AlterColumn("dbo.Leads", "LeadId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "ActivityId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Leads", "Id");
            AddPrimaryKey("dbo.Activities", "Id");
        }
    }
}
