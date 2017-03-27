using System.Collections.Generic;
using PGSBoard.Dtos;

namespace PGSBoard.ViewModels
{

    //This viewModel contains data for showBoard view - it contains name of board, collection of list which board contains and 
    //DTO for creating new list which will be filled by data from form

    public class ShowBoardViewModel
    {
        public string Name { get; set; }
        public List<ListViewModel> Lists { get; set; }
        public CreateListDto CreateListDto { get; set; }

        public ShowBoardViewModel(string name, List<ListViewModel> lists, CreateListDto createListDto)
        {
            Name = name;
            Lists = lists;
            CreateListDto = createListDto;
        }
    }
}