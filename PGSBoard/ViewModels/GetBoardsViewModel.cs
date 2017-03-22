namespace PGSBoard.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using PGSBoard.Models;

    public class GetBoardsViewModel
    {
        public GetBoardsViewModel(List<Board> boards)
        {
            this.BoardsItems = new System.Web.Mvc.SelectList(boards, "Id", "Name");
        }

        public int SelectedBoardId { get; set; }
        public IEnumerable<SelectListItem> BoardsItems { get; set; }
    }
}