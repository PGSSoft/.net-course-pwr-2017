using System.Collections.Generic;
using PGSBoard.Dtos;

namespace PGSBoard.ViewModels
{
    //This is view model of list, and it contains: name, collection of cards and DTO for creating new card

    public class ListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CardViewModel> Cards { get; set; }
        public CreateCardDto CreateCardDto { get; set; }
        public ListViewModel(string name, List<CardViewModel> cards, CreateCardDto createCardDto, int id)
        {
            Name = name;
            Cards = cards;
            CreateCardDto = createCardDto;
            Id = id;
        }
    }
}