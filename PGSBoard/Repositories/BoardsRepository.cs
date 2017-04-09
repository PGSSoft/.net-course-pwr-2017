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

        public void UpdateCardPosition(UpdateCardPositionDto updateCardPositionDto) // during save position card you could meet two cases
        {
            using (var db = new PGSBoardContext())
            {
                if (updateCardPositionDto.ListId == updateCardPositionDto.OldListId) // first case when you sort cart in the same list
                {
                    var currentListCard = db.Cards.Where(card => card.ListId == updateCardPositionDto.ListId).ToList(); // take whole card from list, just one call to db
                    var cardToUpdate = currentListCard.Single(card => card.Id == updateCardPositionDto.CardId); // take  card to Update (card which we drage and drop)
                    var cardChangedAutomatically =                                                               // select card which have to change position after our drag and drop move
                        currentListCard.Single(card => card.PositionCardId == updateCardPositionDto.PositionCard);

                    cardChangedAutomatically.PositionCardId = cardChangedAutomatically.PositionCardId > cardToUpdate.PositionCardId // 2 cases: First, if you take card from up to down then position card which have to change  will be smaller about -1
                        ? cardChangedAutomatically.PositionCardId - 1                                                               // Second, if you take card from down to up, then position card which have to change will be larger about +1
                        : cardChangedAutomatically.PositionCardId + 1;          
                    cardToUpdate.PositionCardId = updateCardPositionDto.PositionCard; // update position card which we drag and drop
                    db.SaveChanges();

                }
                else // second case when you sort( drag some card) to another list 
                {
                    var cards = db.Cards.Where(card => card.ListId == updateCardPositionDto.ListId || card.ListId == updateCardPositionDto.OldListId).ToList(); //take card from two lists
                    var cardToUpdate = cards.Single(card => card.Id == updateCardPositionDto.CardId); // take our card which we drag and drop
                    var prevCardsList = 
                        cards
                        .Where(card => card.ListId == updateCardPositionDto.OldListId  //take old list, where was our card
                        && card.PositionCardId > cardToUpdate.PositionCardId  // take all card which had larger position
                        && card.Id != updateCardPositionDto.CardId).ToList(); // don't take card which we drag and drop
                    var nextCardsList =
                        cards
                        .Where(card => card.ListId == updateCardPositionDto.ListId // take new list, where we drop our card
                        && card.PositionCardId >= updateCardPositionDto.PositionCard).ToList(); // take card where position is larger than our dropped card

                    foreach (var prevCard in prevCardsList)
                    {
                        prevCard.PositionCardId = prevCard.PositionCardId - 1; // old list, reduce position about -1
                    }

                    foreach (var nextCard in nextCardsList)
                    {
                        nextCard.PositionCardId = nextCard.PositionCardId + 1; // new list increase position card about 1
                    }
                    cardToUpdate.ListId = updateCardPositionDto.ListId;
                    cardToUpdate.PositionCardId = updateCardPositionDto.PositionCard; //update dropped position card

                    db.SaveChanges();
                }
            }
        }

        public int ListLength(int listId)
        {
            using (var db = new PGSBoardContext())
            {
                var listLength = db.Cards.Count(card => card.ListId == listId);  // get number of cards in one list
                return listLength;
            }
        }

        public void ChangePositionCardBeforeDelete(DeleteCardDto dto) // reduce position cards after delete
        {
            using (var db = new PGSBoardContext())
            {
                var cards = db.Cards.Where(card => card.ListId == dto.ListId || card.Id == dto.CardId).ToList();
                var cardToDelete = cards.Single(card => card.Id == dto.CardId);
                var cardToUpdate = cards.Where(card => card.Id != dto.CardId && card.PositionCardId > cardToDelete.PositionCardId).ToList();

                foreach (var card in cardToUpdate)
                { 
                    card.PositionCardId = card.PositionCardId - 1; 
                }
                db.SaveChanges();
            }
        }
    }
}