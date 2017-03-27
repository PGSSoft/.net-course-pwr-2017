using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGSBoard.Dtos
{
    public class CreateListDto
    {
        public int BoardId { get; set; }
        public string Name { get; set; }

        public CreateListDto()
        {
            
        }
        public CreateListDto(int boardId)
        {
            BoardId = boardId;
        }
    }
}