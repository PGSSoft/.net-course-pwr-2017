﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGSBoard.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<List> Lists { get; set; }

        public Board()
        {
            Lists = new List<List>();
        }
    }
}