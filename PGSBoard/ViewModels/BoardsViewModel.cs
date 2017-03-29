using PGSBoard.Dtos;

namespace PGSBoard.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BoardsViewModel
    {
        public BoardsViewModel(IEnumerable<BoardDto> boards)
        {
            this.BoardsItems = new SelectList(boards, "Id", "Name");
        }

        public int SelectedBoardId { get; set; }

        public IEnumerable<SelectListItem> BoardsItems { get; set; }
    }
}