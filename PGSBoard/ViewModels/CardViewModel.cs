namespace PGSBoard.ViewModels
{

    //This is viewModel for card, it contains only card data: name and description

    public class CardViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CardViewModel(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}