using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBook.Models;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FPTBook.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly FPTBookIdentityDbContext _context;

        public CategoriesController(FPTBookIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        [Authorize(Roles = "StoreOwner, Admin")]
        public async Task<IActionResult> Index()
        {
              return _context.Category != null ? 
                          View(await _context.Category.ToListAsync()) :
                          Problem("Entity set 'FPTBookContext.Category'  is null.");
        }

        [Authorize(Roles = "StoreOwner, Admin")]
        public async Task<IActionResult> RequestCategory()
        {
              return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "StoreOwner, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "StoreOwner, Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] Category category, string Name)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                if(_context.Category.Where(a => a.Name == Name).ToList().Count != 0){
                     return View(category);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RequestCategory));
            }
            return View(category);
        }
    }
}
