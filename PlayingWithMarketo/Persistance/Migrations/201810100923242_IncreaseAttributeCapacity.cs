namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseAttributeCapacity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeadActivities", "Attributes", c => c.String(maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeadActivities", "Attributes", c => c.String(maxLength: 2000));
        }
    }
}
