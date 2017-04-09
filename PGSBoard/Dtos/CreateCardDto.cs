namespace PGSBoard.Dtos
{
    public class CreateCardDto
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ListLength { get; set; }

        public CreateCardDto()
        {
            
        }

        public CreateCardDto(int listId)
        {
            ListId = listId;
        }
    }
}