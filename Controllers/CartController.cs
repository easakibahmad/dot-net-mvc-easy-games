using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "Cart";

        public CartController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult Add(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.StockId == id);
            if (item != null) item.Quantity++;
            else cart.Add(new CartItem { StockId = stock.Id, Name = stock.Name, Price = stock.Price, Quantity = 1 });

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            HttpContext.Session.Remove(CartKey);
            return View("Success");
        }

        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartKey);
            return cart ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartKey, cart);
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
            => session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
