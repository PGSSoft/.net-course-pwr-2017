using System.Collections.Generic;

namespace PGSBoard.Dtos
{
    public class BoardDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<ListDto> Lists { get; set; }
    }
}