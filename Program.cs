using EasyGames.Data;
using EasyGames.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EasyGamesDB"));

// Enable session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed Stocks
    if (!context.Stocks.Any())
    {
        context.Stocks.AddRange(
            new Stock { Name = "Harry Potter Book", Category = "Book", Quantity = 10, Price = 15.99M },
            new Stock { Name = "Monopoly Game", Category = "Game", Quantity = 8, Price = 25.50M },
            new Stock { Name = "LEGO Set", Category = "Toy", Quantity = 12, Price = 35.00M },
            new Stock { Name = "Chess Board", Category = "Game", Quantity = 7, Price = 20.00M },
            new Stock { Name = "Toy Car", Category = "Toy", Quantity = 15, Price = 10.00M },
            new Stock { Name = "Science Book", Category = "Book", Quantity = 5, Price = 18.00M }
        );
    }

    // Seed Users
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Username = "alice", Email = "alice@example.com", Password = "12345" },
            new User { Username = "bob", Email = "bob@example.com", Password = "12345" },
            new User { Username = "charlie", Email = "charlie@example.com", Password = "12345" },
            new User { Username = "david", Email = "david@example.com", Password = "12345" },
            new User { Username = "eve", Email = "eve@example.com", Password = "12345" },
            new User { Username = "frank", Email = "frank@example.com", Password = "12345" }
        );
    }

    context.SaveChanges();
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
