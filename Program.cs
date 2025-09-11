using EasyGames.Data;
using EasyGames.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Use Neon PostgreSQL instead of InMemory
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Seed initial data if DB is empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply migrations automatically
    context.Database.Migrate();

    // Seed Stocks
    if (!context.Stocks.Any())
    {
        context.Stocks.AddRange(
            new Stock { Name = "BOOK", Category = "Book", Quantity = 10, Price = 19.99M },
            new Stock { Name = "GAME", Category = "Game", Quantity = 7, Price = 39.99M },
            new Stock { Name = "TOY", Category = "Toy", Quantity = 15, Price = 19.99M }
        );
    }

    // Seed Users
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Username = "owner", Email = "owner@email.com", Password = "owner123", Role = "Owner" },
            new User { Username = "user", Email = "user@email.com", Password = "user123", Role = "User" },
            new User { Username = "user2", Email = "user2@email.com", Password = "user123", Role = "User" }
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
