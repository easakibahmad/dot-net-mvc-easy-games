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
            // Books
            new Stock { Name = "Intermezzo", Category = "Book", Quantity = 10, Price = 19.99M },
            new Stock { Name = "The Season", Category = "Book", Quantity = 8, Price = 24.99M },
            new Stock { Name = "Juice", Category = "Book", Quantity = 12, Price = 29.99M },

            // Games
            new Stock { Name = "KILL KNIGHT", Category = "Game", Quantity = 7, Price = 39.99M },
            new Stock { Name = "The Plucky Squire", Category = "Game", Quantity = 10, Price = 49.99M },
            new Stock { Name = "CONSCRIPT", Category = "Game", Quantity = 5, Price = 29.99M },

            // Toys
            new Stock { Name = "Hot Wheels 5-Car Pack Assortment", Category = "Toy", Quantity = 15, Price = 19.99M },
            new Stock { Name = "LEGO Creator Flatbed Truck with Helicopter", Category = "Toy", Quantity = 20, Price = 49.99M },
            new Stock { Name = "Rubik's Cube The Original", Category = "Toy", Quantity = 25, Price = 14.99M }
        );
    }

    // Seed Users
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Username = "elizabeth_blackburn", Email = "elizabeth.blackburn@example.com", Password = "12345" },
            new User { Username = "william_bragg", Email = "william.bragg@example.com", Password = "12345" },
            new User { Username = "frank_burnet", Email = "frank.burnet@example.com", Password = "12345" },
            new User { Username = "graeme_clark", Email = "graeme.clark@example.com", Password = "12345" },
            new User { Username = "ian_clunies_ross", Email = "ian.clunies_ross@example.com", Password = "12345" },
            new User { Username = "michelle_simmons", Email = "michelle.simmons@example.com", Password = "12345" }
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
