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

     [Authorize(Roles = "Customer, StoreOwner, Admin")]
    public async Task<IActionResult> PlaceOrderAsync(decimal total, string fullname, string address, string phone, string cusid, int quantity)
    {
        ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
        Order myOrder = new Order();
        myOrder.OrderTime = DateTime.Now;
        myOrder.Total = total;
        myOrder.Fullname = fullname;
        myOrder.Address = address;
        myOrder.Phone = phone;        
        myOrder.UserID = cusid;
        myOrder.Quantity = quantity;
        myOrder.State = "Delivering";

        _context.Order.Add(myOrder);
        _context.SaveChanges();//this generates the Id for Order

        foreach (var item in cart.Items)
        {
            var bookDb = _context.Book.Find(item.ID);
            bookDb.Quantity = bookDb.Quantity - item.Quantity;
            if(bookDb.Quantity < 0){
                return View("ErrorQuantity");
            }
            else{
            _context.Update(bookDb);
            OrderItem myOrderItem = new OrderItem();
            myOrderItem.BookID = item.ID;
            myOrderItem.Quantity = item.Quantity;
            myOrderItem.OrderID = myOrder.Id;//id of saved order above
            _context.OrderItem.Add(myOrderItem);
            }
        }
        _context.SaveChanges();

        //empty shopping cart
        cart = new ShoppingCart();
        HttpContext.Session.SetObject("cart", cart);
        return View();
    }

    [HttpPost]
    public RedirectToActionResult EditOrder(int id, int quantity)
    {
        ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
        cart.EditItem(id, quantity);
        HttpContext.Session.SetObject("cart", cart);

        return RedirectToAction("CheckOut", "Home");
    }
    [HttpPost]
    public RedirectToActionResult RemoveOrderItem(int id)
    {
        ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
        cart.RemoveItem(id);
        HttpContext.Session.SetObject("cart", cart);

        return RedirectToAction("CheckOut", "Home");
    }

    [Authorize(Roles = "Customer, StoreOwner, Admin")]
    public async Task<IActionResult> Record()
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return _context.Order != null ?
                    View(await _context.Order
                    .Where(o => o.UserID == userID)
                    .ToListAsync()) :
                    Problem("Entity set 'FPTBookContext.Order'  is null.");
    }
    [Authorize(Roles = "Customer, StoreOwner, Admin")]
    public async Task<IActionResult> OrderDetail(int id)
    {
        var fPTBookContext = _context.OrderItem.Where(e => e.Order.Id == id).Include(b => b.Book).Include(o => o.Order).Include(c => c.Book.Author);
        return View(await fPTBookContext.ToListAsync());
    }
}
