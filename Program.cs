using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.InMemory;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;
using Reina.MacCredy.Services;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Get the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if running in a Docker container
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var dbProvider = builder.Configuration["DatabaseProvider"] ?? "SQLite";

// Configure database context based on environment
if (isDocker || dbProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    // When running in Docker, use SQL Server
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(connectionString, sqlOptions => 
            sqlOptions.MigrationsAssembly("Reina.MacCredy")));
    
    builder.Services.AddHealthChecks()
        .AddSqlServer(connectionString, name: "sqlserver");
        
    Console.WriteLine("Using SQL Server database provider");
}
else
{
    // Default to SQLite for local development
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlite("Data Source=cafe_shop.db", sqliteOptions => 
            sqliteOptions.MigrationsAssembly("Reina.MacCredy")));
    
    builder.Services.AddHealthChecks()
        .AddSqlite("Data Source=cafe_shop.db", name: "sqlite");
        
    Console.WriteLine("Using SQLite database provider");
}

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddHttpClient();  // Add HttpClient factory

// Add HttpContextAccessor for permission checks
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>(); // Declare logger once here
    try
    {
        logger.LogInformation("Attempting to get ApplicationDbContext...");
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        if (isDocker || dbProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("ApplicationDbContext obtained. Using SQL Server database.");
            
            // For SQL Server, apply migrations
            context.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("ApplicationDbContext obtained. Using SQLite database.");
            
            // For SQLite, use EnsureCreated
            context.Database.EnsureCreated();
            logger.LogInformation("Database created successfully.");
        }

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "User" }; // Add your desired roles

        logger.LogInformation("Attempting to check/create roles...");
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }
        logger.LogInformation("Role check/creation completed.");

        // Create some test products if database is empty
        if (!context.Products.Any())
        {
            logger.LogInformation("Creating test products...");
            
            // Create test categories first
            var categories = new List<Category>
            {
                new Category { Name = "Hot Coffee", Description = "Hot coffee beverages" },
                new Category { Name = "Cold Coffee", Description = "Cold and iced coffee beverages" },
                new Category { Name = "Specialty Drinks", Description = "Special tea and non-coffee drinks" },
                new Category { Name = "Food", Description = "Bakery and snack items" }
            };
            
            context.Categories.AddRange(categories);
            context.SaveChanges();
            
            // Now add products with categories
            var products = new List<Product>
            {
                new Product {
                    Name = "Traditional Vietnamese Coffee",
                    Description = "Authentic Vietnamese coffee brewed with a traditional filter, rich and strong in flavor.",
                    Price = 38000,
                    CategoryId = 1, // Hot Coffee
                    ImageUrl = "/images/coffee/CafeTruyenThong.jpg",
                    IsAvailable = true,
                    CanCustomize = true,
                    HasSizeOptions = true,
                    PrepTime = "3-4 min",
                    IsFeatured = true
                },
                new Product {
                    Name = "Hong Tra Tea",
                    Description = "A refreshing black tea with a rich aroma and smooth flavor profile.",
                    Price = 42000,
                    CategoryId = 3, // Specialty Drinks
                    ImageUrl = "/images/coffee/HongTra.jpg",
                    IsAvailable = true,
                    CanCustomize = true,
                    HasSizeOptions = true,
                    PrepTime = "2-3 min",
                    IsFeatured = true
                },
                new Product {
                    Name = "Iced Caramel Macchiato",
                    Description = "Espresso with vanilla syrup, milk and caramel sauce over ice.",
                    Price = 48000,
                    CategoryId = 2, // Cold Coffee
                    ImageUrl = "/images/coffee/MacchiatoCaramel.jpg",
                    IsAvailable = true,
                    CanCustomize = true,
                    HasSizeOptions = true,
                    PrepTime = "2-3 min",
                    IsFeatured = true
                }
            };
            
            context.Products.AddRange(products);
            context.SaveChanges();
            logger.LogInformation("Test products created successfully.");
        }
    }
    catch (Exception ex)
    {
        // Use the logger declared in the outer scope
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // In development mode we'll use more detailed error pages
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map health endpoint
app.MapHealthChecks("/health");

app.Run();
