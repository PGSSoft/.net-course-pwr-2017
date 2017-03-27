using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PGSBoard.Models
{
    public class Board
    {
        public Board()
        {
            Lists = new HashSet<List>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public virtual ICollection<List> Lists { get; private set; }
    }
}