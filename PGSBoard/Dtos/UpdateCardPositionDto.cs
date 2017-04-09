using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGSBoard.Dtos
{
    public class UpdateCardPositionDto
    {
        public int CardId { get; set; }
        public int ListId { get; set; }
        public int PositionCard { get; set; }

        public int OldListId { get; set; }
    }
}