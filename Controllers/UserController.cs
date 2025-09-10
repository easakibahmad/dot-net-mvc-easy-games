using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using EasyGames.Models;

namespace EasyGames.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                TempData["UserCreateError"] = "Username already exists. Please choose a different one.";
                return RedirectToAction("Create");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            TempData["UserCreateSuccess"] = "User created successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            TempData["UserUpdate"] = $"User '{user.Username}' was updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["UserDelete"] = $"User '{user.Username}' was deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Login() => View();
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                TempData["Username"] = user.Username; 
                TempData["Role"] = user.Role; 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
