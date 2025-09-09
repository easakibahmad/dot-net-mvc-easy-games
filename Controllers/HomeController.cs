using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            // Show all stock to users
            var stock = _context.Stocks.ToList();
            return View(stock);
        }

        // User Registration
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "User"; 
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["Username"] = user.Username;
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
