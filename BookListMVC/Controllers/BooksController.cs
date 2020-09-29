using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDBContext _db;
        [BindProperty]
        public Book Book { get; set; }
        public BooksController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            //create
            if (id == null)
                return View(Book);
            //update
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
                return NotFound();
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                    _db.Books.Add(Book);
                else
                    _db.Books.Update(Book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }


        #region Api Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(n => n.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Failed to delete" });
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
