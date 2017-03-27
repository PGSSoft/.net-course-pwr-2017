namespace PGSBoard.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PGSBoard.DBContexts.PGSBoardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PGSBoard.DBContexts.PGSBoardContext";
        }

        protected override void Seed(PGSBoard.DBContexts.PGSBoardContext context)
        {
            context.Boards.AddOrUpdate(
              b => b.Name,
                new Board { Name = "New Board", Description = "New Description" },
                new Board { Name = "New Board", Description = "New Description" },
                new Board { Name = "New Board", Description = "New Description" }
            );
        }
    }
}
