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
        
        // static constructor because we don't want to create GetBoardsViewModel on each request
        static BoardController()
        {
            GetBoardsViewModel = 
                new GetBoardsViewModel();
        }

        // GET: return the main view of our app with GetBoardsViewModel (to fill the view)
        public ActionResult Index()
        {
            return View(GetBoardsViewModel);
        }
        
        // GET method that return view of creating board form  
        public ActionResult CreateBoard()
        {
            return View();
        }

        // POST method that takes Board as an argument and adds it to the list of boards
        // and then it redirects you to Index view (you can check that your new Board is available there)
        [HttpPost]
        public ActionResult CreateBoard(Board board)
        {
            GetBoardsViewModel.Boards.Add(board);
            return RedirectToAction("Index");
        }
    }

    // View model that allows you to deliever list of boards to Index view
    public class GetBoardsViewModel
    {
        public List<Board> Boards { get; set; }
        public int SelectedBoardId { get; set; } = 3;   // it means that board with id = 3 will be selected
        public IEnumerable<SelectListItem> BoardsItems // we user IEnumerable<SelectListItem> because drop-down needs that type
            => new SelectList(Boards, "Id", "Name");   // we tell the SelectList where to find the Board to be displayed 
                                                       // "Id" - name of Board's property that should be treated as a value of drop-down
                                                       // "Name" - name of Board's property that should be displayed in drop-down options 
        // constructor
        public GetBoardsViewModel() 
        {
            // creation of list of boards
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
