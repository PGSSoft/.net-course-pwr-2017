using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGSBoard.Models
{
    public class List
    {
        public List()
        {
            Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Board Board { get; set; }

        public int BoardId { get; set; }

        public virtual ICollection<Card> Cards { get; private set; }
    }
}