using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FPTBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using FPTBook.Utils;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FPTBook.Controllers;

public class HomeController : Controller
{
    private readonly FPTBookIdentityDbContext _context;
    private readonly UserManager<BookUser> _userManager;

    public HomeController(FPTBookIdentityDbContext context, UserManager<BookUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        // var fPTBookContext = _context.Book.Include(b => b.Category);
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

      public async Task<IActionResult> Detail(int? id)
    {
        if (id == null || _context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

     public IActionResult Help()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }

    
}
