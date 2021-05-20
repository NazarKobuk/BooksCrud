using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksCrud.Models;
using BooksCrud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BooksCrud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AdminController : Controller
    {
        readonly ApplicationContext _db;
        public AdminController(ApplicationContext db)
        {
            _db = db;
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            return Json(_db.Users.ToList());
        }
    }
}
