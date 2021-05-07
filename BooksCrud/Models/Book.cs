using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksCrud.Models
{
    public class Book
    {
        
        
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
       
        public string Year { get; set; }
        public int Downloads { get; set; }

    }
}
