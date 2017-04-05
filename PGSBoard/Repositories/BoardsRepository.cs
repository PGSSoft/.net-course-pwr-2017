using PGSBoard.DBContexts;
using PGSBoard.Models;
using System.Collections.Generic;
using System.Linq;
using PGSBoard.Dtos;

namespace PGSBoard.Repositories
{
    public class BoardsRepository
    {
        public IEnumerable<BoardDto> GetBoards()
        {
            using(var db = new PGSBoardContext())
            {
                var boards = db.Boards
                    .Include("Lists.Cards")
                    .ToList();

                // dto mapping
                var boardsDto = boards.Select(b => new BoardDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Lists = b.Lists.Select(l => new ListDto
                    {
                        Id = l.Id,
                        Name = l.Name,
                        Cards = l.Cards.Select(c => new CardDto
                        {
                            Name = c.Name,
                            Description = c.Description
                        })
                    })
                });

                return boardsDto;
            }
        }

        public void AddBoard(CreateBoardFormDto dto)
        {
            var board = new Board
            {
                Name = dto.Name,
                Description = dto.Description
            };

            using(var db = new PGSBoardContext())
            {
                db.Boards.Add(board);
                db.SaveChanges();
            }
        }

        public BoardDto GetBoard(int boardId)
        {
            using(var db = new PGSBoardContext())
            {
                var board = db.Boards
                    .Include("Lists.Cards")
                    .Single(b => b.Id == boardId);

                // dto mapping
                var boardDto = new BoardDto
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    Lists = board.Lists.Select(l => new ListDto
                    {
                        Id = l.Id,
                        Name = l.Name,
                        Cards = l.Cards.Select(c => new CardDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description
                        })
                    })
                };

                return boardDto;
            }
        }

        public void AddList(CreateListDto dto)
        {
            var list = new List
            {
                Name = dto.Name,
                BoardId = dto.BoardId
            };

            using (var db = new PGSBoardContext())
            {
                db.Lists.Add(list);
                db.SaveChanges();
            }
        }

        public void AddCard(CreateCardDto dto)
        {
            var card = new Card
            {
                Name = dto.Name,
                Description = dto.Description,
                ListId = dto.ListId
            };

            using (var db = new PGSBoardContext())
            {
                db.Cards.Add(card);
                db.SaveChanges();
            }
        }


        public int DeleteCard(DeleteCardDto dto)
        {
            using (var db = new PGSBoardContext())
            {
                var cardToRemove = db.Cards.Single(card => card.Id == dto.CardId);
                db.Cards.Remove(cardToRemove);
                return db.SaveChanges() == 1 ? dto.CardId : 0;
            }
        }
    }
}