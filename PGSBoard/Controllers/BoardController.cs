using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PGSBoard.Models;

namespace PGSBoard.Controllers
{
    public class BoardController : Controller
    {
        public static GetBoardsViewModel GetBoardsViewModel { get; set; }
        // GET: Board
        static BoardController()
        {
            GetBoardsViewModel = 
                new GetBoardsViewModel();
        }

        public ActionResult Index()
        {
            return View(GetBoardsViewModel);
        }

        public ActionResult CreateBoard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBoard(Board board)
        {
            GetBoardsViewModel.Boards.Add(board);
            return RedirectToAction("Index");
        }
    }

    public class GetBoardsViewModel
    {
        public List<Board> Boards { get; set; }
        public int SelectedBoardId { get; set; } = 3;
        public IEnumerable<SelectListItem> BoardsItems 
            => new SelectList(Boards, "Id", "Name");
        public GetBoardsViewModel()
        {
            Boards = new List<Board>
            {
                new Board { Id = 1, Name = "First", Description = "xOXO"},
                new Board { Id = 2, Name = "Second", Description = "xOXO2"},
                new Board { Id = 3, Name = "Third", Description = "xOXO3"},
                new Board { Id = 4, Name = "Fourth", Description = "xOXO4"}
            };
        }
    }
}