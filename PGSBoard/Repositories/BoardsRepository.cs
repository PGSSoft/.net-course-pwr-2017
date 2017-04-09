using PGSBoard.DBContexts;
using PGSBoard.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using PGSBoard.Dtos;
using WebGrease.Css.Extensions;

namespace PGSBoard.Repositories
{
    public class BoardsRepository
    {
        public IEnumerable<BoardDto> GetBoards()
        {
            using (var db = new PGSBoardContext())
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

            using (var db = new PGSBoardContext())
            {
                db.Boards.Add(board);
                db.SaveChanges();
            }
        }

        public BoardDto GetBoard(int boardId)
        {
            using (var db = new PGSBoardContext())
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
                            Description = c.Description,
                            PositionCardId = c.PositionCardId
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
                ListId = dto.ListId,
                PositionCardId = dto.ListLength > 0 ? dto.ListLength : 0
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

        public int DeleteList(DeleteListDto deleteListDto)
        {
            using (var db = new PGSBoardContext())
            {
                var listToRemove = db.Lists.Single(list => list.Id == deleteListDto.ListId);
                db.Lists.Remove((listToRemove));
                return db.SaveChanges() == 1 ? deleteListDto.ListId : 0;
            }
        }

        public void UpdateCardPosition(UpdateCardPositionDto updateCardPositionDto)
        {
            using (var db = new PGSBoardContext())
            {
                if (updateCardPositionDto.ListId == updateCardPositionDto.OldListId)
                {
                    var currentListCard = db.Cards.Where(card => card.ListId == updateCardPositionDto.ListId).ToList();
                    var cardToUpdate = currentListCard.Single(card => card.Id == updateCardPositionDto.CardId);
                    var cardChangedAutomatically =
                        currentListCard.Single(card => card.PositionCardId == updateCardPositionDto.PositionCard);

                    cardChangedAutomatically.PositionCardId = cardChangedAutomatically.PositionCardId >
                                                              cardToUpdate.PositionCardId
                        ? cardChangedAutomatically.PositionCardId - 1
                        : cardChangedAutomatically.PositionCardId + 1;
                    cardToUpdate.ListId = updateCardPositionDto.ListId;
                    cardToUpdate.PositionCardId = updateCardPositionDto.PositionCard;
                    db.SaveChanges();

                }
                else
                {
                    var cards = db.Cards.Where(card => card.ListId == updateCardPositionDto.ListId || card.ListId == updateCardPositionDto.OldListId).ToList();
                    var cardToUpdate = cards.Single(card => card.Id == updateCardPositionDto.CardId);
                    var prevCardsList = cards.Where(card => card.ListId == updateCardPositionDto.OldListId && card.PositionCardId > cardToUpdate.PositionCardId && card.Id != updateCardPositionDto.CardId).ToList();
                    var nextCardsList = cards.Where(card => card.ListId == updateCardPositionDto.ListId && card.PositionCardId >= updateCardPositionDto.PositionCard).ToList();
                    foreach (var prevCard in prevCardsList)
                    {
                        prevCard.PositionCardId = prevCard.PositionCardId - 1;
                    }

                    foreach (var nextCard in nextCardsList)
                    {
                        nextCard.PositionCardId = nextCard.PositionCardId + 1;
                    }
                    cardToUpdate.ListId = updateCardPositionDto.ListId;
                    cardToUpdate.PositionCardId = updateCardPositionDto.PositionCard;

                    db.SaveChanges();
                }
            }
        }

        public int ListLength(int listId)
        {
            using (var db = new PGSBoardContext())
            {
                var listLength = db.Cards.Count(card => card.ListId == listId);
                return listLength;
            }
        }

        public void ChangePositionCardBeforeDelete(DeleteCardDto dto)
        {
            using (var db = new PGSBoardContext())
            {
                var cardToDelete = db.Cards.Single(card => card.Id == dto.CardId);
                var cardToUpdate =
                    db.Cards.Where(card => card.Id != dto.CardId && card.PositionCardId > cardToDelete.PositionCardId && card.ListId == dto.ListId)
                        .ToList();

                foreach (var card in cardToUpdate)
                {
                    card.PositionCardId = card.PositionCardId - 1;
                }
                db.SaveChanges();
            }
        }
    }
}