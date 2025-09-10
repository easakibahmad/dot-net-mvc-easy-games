using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShopController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var stocks = _context.Stocks.ToList();
            return View(stocks);
        }
    }
}
