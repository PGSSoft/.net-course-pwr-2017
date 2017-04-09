using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGSBoard.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        public string Name  { get; set; }

        public string Description { get; set; }

        public int ListId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionCardId { get; set; }

        public List List { get; set; }
    }
}