using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Stocks.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Stock stock)
        {
            _context.Stocks.Add(stock);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var stock = _context.Stocks.Find(id);
            return stock == null ? NotFound() : View(stock);
        }

        [HttpPost]
        public IActionResult Edit(Stock stock)
        {
            _context.Stocks.Update(stock);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
