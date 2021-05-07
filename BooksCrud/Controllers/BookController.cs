using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksCrud.Models;
using BooksCrud.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksCrud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : Controller
    {
        readonly ApplicationContext _db;
        public BookController(ApplicationContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetBook()
        {
            return Json(_db.Books.ToList());
        }

        [HttpGet]
        public IActionResult GetOneBook(GetOneBook book)
        {
            if(_db.Books.Where(x => x.Title == book.Title).Count() > 0)
            {
                
                return Json(_db.Books.Where(x => x.Title == book.Title));
            }
            else
            {
                return BadRequest(new { error = "There is no book with this title" });
            }
        }

        [HttpPost]
        public IActionResult PostBook(CreateBook book)
        {
            if (Int32.Parse(book.Year) > DateTime.Now.Year || Int32.Parse(book.Year) < 0)
            {
                return BadRequest(new { message = "Please, select valid year!" });

            }

            if (_db.Books.Where(x => x.Title == book.Title).Count() > 0 && _db.Books.Where(x => x.Author == book.Author).Count() > 0)
            {
                return BadRequest(new { error = "Such book is already exists" });
            }

            try
            {
                Book bk = new Book { Author = book.Author, Title = book.Title, Year = book.Year };
                _db.Books.Add(bk);
                _db.SaveChanges();
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult DownloadBook(DownloadBook book)
        {
            Book bk = _db.Books.Find(book.Id);
            bk.Downloads += 1;
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult PutBook(CreateBook book)
        {
            
                
            Book bk = _db.Books.Find(book.Id);
            bk.Title = book.Title;
            bk.Year = book.Year;
            bk.Author = book.Author;
            _db.SaveChanges();
            return Ok();
               
            
            
        }

        [HttpDelete]
        public IActionResult DeleteBook(int Id)
        {
            _db.Books.Remove(_db.Books.Find(Id));
            _db.SaveChanges();
            return Ok();
        }
    }
}
