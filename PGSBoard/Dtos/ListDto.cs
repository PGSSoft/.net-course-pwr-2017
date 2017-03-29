using System.Collections.Generic;
using PGSBoard.Models;

namespace PGSBoard.Dtos
{
    public class ListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<CardDto> Cards { get; set; }
    }
}