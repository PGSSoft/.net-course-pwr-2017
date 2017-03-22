namespace PGSBoard.Controllers
{
    using System.Web.Mvc;

    using PGSBoard.Dtos;
    using PGSBoard.Models;
    using PGSBoard.Services;
    using PGSBoard.ViewModels;

    public class BoardController : Controller
    {
        private BoardsService _boardsService;

        public BoardController()
        {
            this._boardsService = new BoardsService();
        }
        // GET method that return view of creating board form  
        public ActionResult CreateBoard()
        {
            var viewModel = this._boardsService.GetCreateBoardViewModel();
            return this.View(viewModel);
        }

        // POST method that takes Board as an argument and adds it to the list of boards
        // and then it redirects you to Index view (you can check that your new Board is available there)
        [HttpPost]
        public ActionResult CreateBoard([Bind(Prefix = "CreateBoardFormDto")] CreateBoardFormDto dto)
        {
            if (this.ModelState.IsValid)
            {
                this._boardsService.CreateBoard(dto);
                return RedirectToAction("Index");
            }
            var viewModel = this._boardsService.GetCreateBoardViewModel();
            viewModel.CreateBoardFormDto = dto;
            return View(viewModel);
        }

        // GET: return the main view of our app with GetBoardsViewModel (to fill the view)
        public ActionResult Index()
        {
            var viewModel = _boardsService.GetGetBoardsViewModel();
            return View(viewModel);
        }
    }
}