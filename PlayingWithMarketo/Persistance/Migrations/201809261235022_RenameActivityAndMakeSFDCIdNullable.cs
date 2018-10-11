namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameActivityAndMakeSFDCIdNullable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Acitivities", newName: "Activities");
            AlterColumn("dbo.Leads", "SFDCId", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leads", "SFDCId", c => c.String(nullable: false, maxLength: 25));
            RenameTable(name: "dbo.Activities", newName: "Acitivities");
        }
    }
}
