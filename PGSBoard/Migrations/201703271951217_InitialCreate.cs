namespace PGSBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BoardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId, cascadeDelete: true)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lists", t => t.ListId, cascadeDelete: true)
                .Index(t => t.ListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "ListId", "dbo.Lists");
            DropForeignKey("dbo.Lists", "BoardId", "dbo.Boards");
            DropIndex("dbo.Cards", new[] { "ListId" });
            DropIndex("dbo.Lists", new[] { "BoardId" });
            DropTable("dbo.Cards");
            DropTable("dbo.Lists");
            DropTable("dbo.Boards");
        }
    }
}
