using PGSBoard.DBContexts;
using PGSBoard.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PGSBoard.Dtos;
using System.Diagnostics;

namespace PGSBoard.Repositories
{
    public class BoardsRepository
    {
        public List<Board> GetBoards()
        {
            using(var db = new PGSBoardContext())
            {
                var boards = db.Boards
                    .Include(b => b.Lists.Select(l => l.Cards))
                    .ToList();

                return boards;
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

        public Board GetBoard(int boardId)
        {
            using(var db = new PGSBoardContext())
            {
                var board = db.Boards
                    .Include("Lists.Cards")
                    .Single(b => b.Id == boardId);

                return board;
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
                var newCard = db.Cards
                    .Add(card);
                db.SaveChanges();
            }
        }
    }
}