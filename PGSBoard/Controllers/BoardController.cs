namespace PGSBoard.Controllers
{
    using System.Web.Mvc;

    using PGSBoard.Dtos;
    using PGSBoard.Services;

    public class BoardController : Controller
    {
        private readonly BoardsService _boardsService;   

        public BoardController()
        {
            this._boardsService = new BoardsService(); // We have to initialize boardService
        }
        // GET method that return view of creating board form  
        public ActionResult CreateBoard()
        {
            var viewModel = this._boardsService.GetCreateBoardViewModel();  //We are getting viewModel from service and pass it into view
            return this.View(viewModel);
        }

        // POST method that takes CreateBoardDto checks if it is valid as an argument and calls add CreateBoard moethod from boardsService
        // and then it redirects you to Index view (you can check that your new Board is available there)
        [HttpPost]
        public ActionResult CreateBoard([Bind(Prefix = "CreateBoardFormDto")] CreateBoardFormDto dto)  //We need bind attribute because:
                                    //CreateBoardFormDto is member of ShowBoardsViewModel which is model of Create view, as you can see in this view
                                    //in helpers texboxfor we tells ASP that this field is for m.CreateBoardFormDto.Name so ASP will name this input field
                                    //CreateBoardFormDto_Name - and there is a problem because CreateBoardFormDto has no field CreateBoardFormDto_Name, it has
                                    //only Name field so we need tell ASP that in this form names there is a prefix CreateBoardFormDto
        {
            if (this.ModelState.IsValid)  //We need to check if our dto is valid (see validation attributes in dto)
            {
                this._boardsService.CreateBoard(dto); //If is valid we are calling create board service
                return RedirectToAction("Index");  //After it redirect to Index view
            }
            var viewModel = this._boardsService.GetCreateBoardViewModel();  //If dto is invalid create viewmodel again
            viewModel.CreateBoardFormDto = dto;  //Pass there invalid dto to not lose information from all fields user filled
            return View(viewModel);   //Show Create board view again
        }

        // GET: return the main view of our app with BoardsViewModel 
        public ActionResult Index()
        {
            var viewModel = _boardsService.GetBoardsViewModel();
            return View(viewModel);
        }

        //GET: it takes from service viewModel of board which id equals SelectedBoardId and displays it
        public ActionResult Show(int SelectedBoardId)
        {
            var viewModel = _boardsService.GetShowBoardViewModel(SelectedBoardId);
            return View(viewModel);
        }

        //POST: Creates new list and redirecting to Show board with id of board to which we have just added list
        [HttpPost]
        public ActionResult CreateList(CreateListDto dto)
        {
            _boardsService.CreateList(dto);
            return RedirectToAction("Show", new { SelectedBoardId = dto.BoardId});
        }

        //POST: Creates new card and redirecting to Show board with id of board to which we have just added card
        [HttpPost]
        public ActionResult CreateCard(CreateCardDto dto)
        {
            int boardId = _boardsService.CreateCart(dto);
            return RedirectToAction("Show", new { SelectedBoardId = boardId });
        }

        //DELETE: Delete card from db and returns if action was successful
        [HttpDelete]
        public JsonResult DeleteCard(int cardId)
        {
            var deleteCardDto = new DeleteCardDto()
            {
                CardId = cardId
            };
            var result = _boardsService.DeleteCard(deleteCardDto);
            return new JsonResult()
            {
                Data = result
            };
        }
    }
}