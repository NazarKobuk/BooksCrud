using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksCrud.ViewModels
{
    public class GetOneBook
    {
        [Required(ErrorMessage = "Title field shouldn't be empty ")]
        public string Title { get; set; }
    }
}
