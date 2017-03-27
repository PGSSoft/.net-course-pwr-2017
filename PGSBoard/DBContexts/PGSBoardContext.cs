using PGSBoard.Models;
using System.Data.Entity;

namespace PGSBoard.DBContexts
{
    public class PGSBoardContext : DbContext
    {
        public PGSBoardContext()
            : base("PGSBoard")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Board> Boards { get; set; }

        public DbSet<List> Lists { get; set; }

        public DbSet<Card> Cards { get; set; }
    }
}