using EasyGames.Data;
using EasyGames.Models;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

// Add services
builder.Services.AddControllersWithViews();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

var owner = Environment.GetEnvironmentVariable("Owner");
var ownerPassword = Environment.GetEnvironmentVariable("OwnerPassword");
var user = Environment.GetEnvironmentVariable("User");
var userPassword = Environment.GetEnvironmentVariable("UserPassword");
var ownerEmail = Environment.GetEnvironmentVariable("OwnerEmail");
var userEmail = Environment.GetEnvironmentVariable("UserEmail");

// Use Neon PostgreSQL instead of InMemory
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

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
            new User { Username = owner, Email = ownerEmail, Password = ownerPassword, Role = "Owner" },
            new User { Username = user, Email = userEmail, Password = userPassword, Role = "User" });
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
