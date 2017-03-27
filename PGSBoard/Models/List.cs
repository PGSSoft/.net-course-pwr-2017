using System.Collections.Generic;

namespace PGSBoard.Models
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }

        public List()
        {
            Cards = new List<Card>();
        }
    }
}