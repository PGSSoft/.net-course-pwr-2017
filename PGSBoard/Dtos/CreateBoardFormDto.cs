using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGSBoard.Dtos
{
    using System.ComponentModel.DataAnnotations;

    //It's DTO for creating new board, this object will be filled with data from create board form
    //This DTO includes validation attributes

    public class CreateBoardFormDto
    {
        [Required(ErrorMessage = "This field is very important so it's required")] //This attribute tells that this field is required and has custom validation message
        public string Name { get; set; }
        [Required]  //This attribute tells that this field is required too so it can't be empty
        [MinLength(5)] //This attribute tells taht this field should be filled with text with at least 5 signs 
        public string Description { get; set; }
    }
}