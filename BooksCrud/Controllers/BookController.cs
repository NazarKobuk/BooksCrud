using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BooksCrud.Models;
using BooksCrud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksCrud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]



    public class BookController : Controller
    {
        readonly ApplicationContext _db;
        readonly IWebHostEnvironment _hostingEnvironment;

        public BookController(ApplicationContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize(Roles = "admin")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }

        [HttpGet]
        [Authorize]

        public IActionResult GetBook()
        {
            return Json(_db.Books.ToList());
        }

        [HttpGet]
        public IActionResult GetOneBook(int Id)
        {
            if(_db.Books.Where(x => x.Id == Id).Count() > 0)
            {
                
                return Json(_db.Books.Where(x => x.Id == Id));
            }
            else
            {
                return BadRequest(new { error = "There is no book with this id" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBook([FromForm] CreateBook book)
        {
            if (Int32.Parse(book.Year) > DateTime.Now.Year || Int32.Parse(book.Year) < 0)
            {
                return BadRequest(new { message = "Please, select valid year!" });

            }

            if (_db.Books.Where(x => x.Title == book.Title).Count() > 0 && _db.Books.Where(x => x.Author == book.Author).Count() > 0)
                {
                return BadRequest(new { error = "Such book is already exists" });
            }

            if (book.BookFile != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "books");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + book.BookFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                book.BookFile.CopyTo(new FileStream(filePath, FileMode.Create));

                Book bk = new Book { Author = book.Author, Title = book.Title, Year = book.Year, BookFilePath = uniqueFileName };
                _db.Books.Add(bk);
                _db.SaveChanges();
                return Ok(new { name = book.BookFile.FileName });
            }
            else
            {
                return BadRequest();
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file != null)
            {

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "books");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Books", file.FileName);
                //var stream = new FileStream(path, FileMode.Create);
                //file.CopyToAsync(stream);
                return Ok(new { lenght = file.Length, name = file.FileName });
            }
            else
            {
                return BadRequest(new { error = "something went wrong" });
            }
        }

        [HttpPost]
        public IActionResult DownloadBook(DownloadBook book)
        {

            if (_db.Books.Where(x => x.Id == book.Id).Count() > 0)
            {

                Book bk = _db.Books.Find(book.Id);
                bk.Downloads += 1;
                _db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new { error = "There is no book to download with this id" });
            }
        }

        [HttpPut]
        public IActionResult PutBook(CreateBook book)
        {
            if (_db.Books.Where(x => x.Id == book.Id).Count() > 0)
            {
                Book bk = _db.Books.Find(book.Id);
                bk.Title = book.Title;
                bk.Year = book.Year;
                bk.Author = book.Author;
                _db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new { error = "There is no book to change with this id" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteBook(int Id)
        {
            if (_db.Books.Where(x => x.Id == Id).Count() > 0)
            {
                _db.Books.Remove(_db.Books.Find(Id));
                _db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new { error = "There is no book to delete with this id" });
            }
        }
    }
}
