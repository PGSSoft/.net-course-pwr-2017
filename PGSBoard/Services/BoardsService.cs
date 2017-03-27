namespace PGSBoard.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using PGSBoard.Dtos;
    using PGSBoard.Models;
    using PGSBoard.ViewModels;
    using Repositories;

    public class BoardsService
    {
        private BoardsRepository repo;

        public BoardsService()
        {
            this.repo = new BoardsRepository();
        }

        //This method creates new GetBoardsViewModel which contains all boards
        public GetBoardsViewModel GetGetBoardsViewModel()
        {
            var boards = this.repo.GetBoards();
            var viewModel = new GetBoardsViewModel(boards);
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
            this.repo.AddBoard(dto);
        }

        //This method search for board with specified id and creates ShowBoardViewModel
        public ShowBoardViewModel GetShowBoardViewModel(int boardId)
        {
            var board = this.repo.GetBoard(boardId);

            var viewModel = new ShowBoardViewModel(board.Name,      //Create viewmodel for Board
                MapListsToListViewModels(board.Lists.ToList()),      //Map all lists which board contains into ListViewModel
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
                listViewModels.Add(new ListViewModel(list.Name, MapCardsToCardViewModels(list.Cards.ToList()),  //Map cards which belongs to list to CardViewModels
                    new CreateCardDto(list.Id))); // Create CreateCardDto for from to creating new card and pass there list id
            }
            return listViewModels;
        }

        //Method for creating new list
        public void CreateList(CreateListDto dto)
        {
            this.repo.AddList(dto);
        }

        //Method for creatin new list
        public int CreateCart(CreateCardDto dto)
        {
            this.repo.AddCard(dto);

            var board = this.repo.GetBoards().Single(x => x.Lists.Select(l => l.Id).Contains(dto.ListId));
            return board.Id;
        }
    }
}