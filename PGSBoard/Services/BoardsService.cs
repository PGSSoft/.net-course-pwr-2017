using System.Linq;
using System.Net.Http.Headers;

namespace PGSBoard.Services
{
    using System.Collections.Generic;

    using PGSBoard.Dtos;
    using PGSBoard.Models;
    using PGSBoard.ViewModels;

    public class BoardsService
    {
        static BoardsService()  // Static constructor which initializes our in memory database
        {
            var lists = new List<List>()
            {
                new List()  // NEW: we are creating lists and cards which belongs to board
                {
                    Id = 0,
                    Name = "List1",
                    Cards = new List<Card>()
                    {
                        new Card()
                        {
                            Id = 0,
                            Name = "Card1",
                            Description = "Card 1 description"
                        },
                        new Card()
                        {
                            Id = 0,
                            Name = "Card2",
                            Description = "Card 2 description"
                        }
                    }
                }
            };
            Boards = new List<Board> //We are creating new boards
                         {
                             new Board { Id = 1, Name = "First", Description = "xOXO", Lists = lists},
                             new Board { Id = 2, Name = "Second", Description = "xOXO2" },
                         };
        }

        public static List<Board> Boards { get; set; }

        //This method creates new GetBoardsViewModel which contains all boards
        public GetBoardsViewModel GetGetBoardsViewModel()
        {
            var viewModel = new GetBoardsViewModel(Boards);
            return viewModel;
        }

        //This method simply creates empty CreateBoardViewModel
        public CreateBoardViewModel GetCreateBoardViewModel()
        {
            var viewModel = new CreateBoardViewModel()
                                {
                                    CreateBoardFormDto = new CreateBoardFormDto()
                                };
            return viewModel;
        }

        //Method for creating new board
        public void CreateBoard(CreateBoardFormDto dto)
        {
            var lastBoard = Boards.OrderBy(x => x.Id).LastOrDefault();  //Get last board (which id is highest)
            var lastId = lastBoard?.Id ?? 0;  //Get last board id, we need it to compute id of new board
            var board = new Board()     //Create new board
                            {
                                Id = lastId+1,
                                Name = dto.Name,
                                Description = dto.Description
                            };
            Boards.Add(board);  //Add it to our "database"
        }

        //This method search for board with specified id and creates ShowBoardViewModel
        public ShowBoardViewModel GetShowBoardViewModel(int boardId)
        {
            var board = Boards.Single(x => x.Id == boardId); // Get from out boards list board with id
            var viewModel = new ShowBoardViewModel(board.Name,      //Create viewmodel for Board
                MapListsToListViewModels(board.Lists),      //Map all lists which board contains into ListViewModel
                new CreateListDto(boardId));    //Create dto for creating new list and assign to it this board id
            return viewModel;
        }

        private List<CardViewModel> MapCardsToCardViewModels(List<Card> cards)  //This method maps cards to CardViewModels
        {
            List<CardViewModel> cardsViewModels = new List<CardViewModel>();
            foreach (var card in cards)
            {
                cardsViewModels.Add(new CardViewModel(card.Name, card.Description));
            }
            return cardsViewModels;
        }

        private List<ListViewModel> MapListsToListViewModels(List<List> lists)  //This method maps lists to ListViewModels
        {
            List<ListViewModel> listViewModels = new List<ListViewModel>();
            foreach (var list in lists)
            {
                listViewModels.Add(new ListViewModel(list.Name, MapCardsToCardViewModels(list.Cards),  //Map cards which belongs to list to CardViewModels
                    new CreateCardDto(list.Id))); // Create CreateCardDto for from to creating new card and pass there list id
            }
            return listViewModels;
        }

        //Method for creating new list
        public void CreateList(CreateListDto dto)
        {
            var board = Boards.Single(x => x.Id == dto.BoardId); // Get board from list to which we need add new list
            var lastList = Boards.SelectMany(x => x.Lists).OrderBy(x => x.Id).LastOrDefault(); // Get last list from all boards to
            var lastId = lastList?.Id ?? 0;                                                 //Compute new list id
            var list = new List()
            {
                Id = lastId + 1,
                Name = dto.Name,
                Cards = new List<Card>()
            };
            board.Lists.Add(list); //Add list to board
        }

        //Method for creatin new list
        public int CreateCart(CreateCardDto dto)
        {
            var list = Boards.SelectMany(x => x.Lists).Single(x => x.Id == dto.ListId); //From lists from all boards select list with id specified in Dto
            var lastCard = Boards.SelectMany(x => x.Lists).SelectMany(x => x.Cards).OrderBy(x => x.Id).LastOrDefault(); //From all boads and all list get last card
            var lastId = lastCard?.Id ?? 0;                                          //To compute new card id
            var card = new Card()
            {
                Id = lastId +1,
                Name = dto.Name,
                Description = dto.Description
            };
            list.Cards.Add(card);  // Add new card to list
            var board = Boards.Single(x => x.Lists.Select(l => l.Id).Contains(dto.ListId));   //Get board with list to which we added card
            return board.Id; //Return id of this board
        }
    }
}