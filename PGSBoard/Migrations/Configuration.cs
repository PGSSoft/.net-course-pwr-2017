namespace PGSBoard.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DBContexts.PGSBoardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PGSBoard.DBContexts.PGSBoardContext";
        }

        protected override void Seed(DBContexts.PGSBoardContext context)
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
