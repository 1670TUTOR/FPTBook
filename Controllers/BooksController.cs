using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using FPTBook.Models;
using FPTBook.Utils;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FPTBook.Controllers
{
    [Authorize(Roles = "StoreOwner, Admin")]
    public class BooksController : Controller
    {
         private readonly FPTBookIdentityDbContext _context;

        private readonly IWebHostEnvironment hostEnvironment;

        public BooksController(FPTBookIdentityDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            hostEnvironment = environment;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString)
        {
            // var fPTBookContext = _context.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            // return View(await fPTBookContext.ToListAsync());
            var fPTBookContext = from m in _context.Book.Include(a => a.Category)
                                                    .Include(b => b.Author)
                                                    .Include(c => c.Publisher)
                                                    select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                fPTBookContext = fPTBookContext.Where(s => s.Title!.Contains(searchString));
            }

            return View(await fPTBookContext.ToListAsync());
        }

         // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        

    }
}
