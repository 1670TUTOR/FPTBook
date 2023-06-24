using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FPTBook.Models;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FPTBook.Controllers;
[Authorize(Roles = "StoreOwner, Admin")]
public class StoreOwnerController : Controller
{
    private readonly ILogger<StoreOwnerController> _logger;

    public StoreOwnerController(ILogger<StoreOwnerController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ManageBook()
    {
        return View();
    }
}
