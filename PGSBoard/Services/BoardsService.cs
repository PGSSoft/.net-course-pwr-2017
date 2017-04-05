namespace PGSBoard.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PGSBoard.Dtos;
    using PGSBoard.Models;
    using PGSBoard.ViewModels;
    using Repositories;

    public class BoardsService
    {
        private readonly BoardsRepository boardsRepository;

        public BoardsService()
        {
            this.boardsRepository = new BoardsRepository();
        }

        //This method creates new BoardsViewModel which contains all boards
        public BoardsViewModel GetBoardsViewModel()
        {
            var boards = this.boardsRepository.GetBoards();
            var viewModel = new BoardsViewModel(boards);
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
            this.boardsRepository.AddBoard(dto);
        }

        //This method search for board with specified id and creates ShowBoardViewModel
        public ShowBoardViewModel GetShowBoardViewModel(int boardId)
        {
            var board = this.boardsRepository.GetBoard(boardId);

            var viewModel = new ShowBoardViewModel(board.Name,      //Create viewmodel for Board
                MapListsToListViewModels(board.Lists.ToList()),      //Map all lists which board contains into ListViewModel
                new CreateListDto(boardId));    //Create dto for creating new list and assign to it this board id
            return viewModel;
        }

        private List<CardViewModel> MapCardsToCardViewModels(List<CardDto> cards)  //This method maps cards to CardViewModels
        {
            List<CardViewModel> cardsViewModels = new List<CardViewModel>();
            foreach (var card in cards)
            {
                cardsViewModels.Add(new CardViewModel(card.Id, card.Name, card.Description));
            }
            return cardsViewModels;
        }

        private List<ListViewModel> MapListsToListViewModels(List<ListDto> lists)  //This method maps lists to ListViewModels
        {
            var listViewModels = new List<ListViewModel>();
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
            this.boardsRepository.AddList(dto);
        }

        //Method for creatin new list
        public int CreateCart(CreateCardDto dto)
        {
            this.boardsRepository.AddCard(dto);

            var boards = this.boardsRepository.GetBoards();
            var board = boards.Single(x => x.Lists.Select(l => l.Id).Contains(dto.ListId));
            return board.Id;
        }

        public int DeleteCard(DeleteCardDto dto)
        {
            return this.boardsRepository.DeleteCard(dto);
        }
    }
}