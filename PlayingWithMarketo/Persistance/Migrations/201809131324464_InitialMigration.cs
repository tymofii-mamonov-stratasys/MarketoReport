namespace PlayingWithMarketo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExportJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportId = c.String(nullable: false),
                        Status = c.String(),
                        Format = c.String(nullable: false, maxLength: 3),
                        CreatedAt = c.DateTime(nullable: false),
                        QueuedAt = c.DateTime(),
                        StartedAt = c.DateTime(),
                        FinishedAt = c.DateTime(),
                        NumberOfRecords = c.Int(),
                        FileSize = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessToken = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpiresAt = c.DateTime(nullable: false),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tokens");
            DropTable("dbo.ExportJobs");
        }
    }
}
