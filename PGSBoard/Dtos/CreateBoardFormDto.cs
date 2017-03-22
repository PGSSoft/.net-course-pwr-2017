using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGSBoard.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class CreateBoardFormDto
    {
        [Required(ErrorMessage = "This field is very important so it's required")]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }
    }
}