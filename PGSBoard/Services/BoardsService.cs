namespace PGSBoard.Services
{
    using System.Collections.Generic;

    using PGSBoard.Dtos;
    using PGSBoard.Models;
    using PGSBoard.ViewModels;

    public class BoardsService
    {
        static BoardsService()
        {
            Boards = new List<Board>
                         {
                             new Board { Id = 1, Name = "First", Description = "xOXO" },
                             new Board { Id = 2, Name = "Second", Description = "xOXO2" },
                             new Board { Id = 3, Name = "Third", Description = "xOXO3" },
                             new Board { Id = 4, Name = "Fourth", Description = "xOXO4" }
                         };
        }

        public static List<Board> Boards { get; set; }

        public GetBoardsViewModel GetGetBoardsViewModel()
        {
            var viewModel = new GetBoardsViewModel(Boards);
            return viewModel;
        }

        public CreateBoardViewModel GetCreateBoardViewModel()
        {
            var viewModel = new CreateBoardViewModel()
                                {
                                    CreateBoardFormDto = new CreateBoardFormDto()
                                };
            return viewModel;
        }

        public void CreateBoard(CreateBoardFormDto dto)
        {
            var board = new Board()
                            {
                                Name = dto.Name,
                                Description = dto.Description
                            };
            Boards.Add(board);
        }
    }
}