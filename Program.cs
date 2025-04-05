using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;
using Reina.MacCredy.Services;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add data protection configuration
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(14));

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure session with explicit cookie settings
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Reina.MacCredy.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure localization
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("vi-VN"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("vi-VN");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

// Add HttpContextAccessor for permission checks
builder.Services.AddHttpContextAccessor();

// Add health checks
builder.Services.AddHealthChecks();

// Add payment service
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>(); // Declare logger once here
    try
    {
        logger.LogInformation("Attempting to get ApplicationDbContext...");
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("ApplicationDbContext obtained. Attempting to migrate database...");

        try
        {
            // Apply migrations at startup with better error handling
            context.Database.Migrate();
            logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception migrationEx)
        {
            logger.LogError(migrationEx, "Error during database migration. Attempting to create database if it doesn't exist...");

            try
            {
                // Try alternate approach if migration fails
                context.Database.EnsureCreated();
                logger.LogInformation("Database created successfully using EnsureCreated.");
            }
            catch (Exception createEx)
            {
                logger.LogError(createEx, "Critical database initialization error. Application may not function correctly.");
                // Continue execution - we'll handle database errors gracefully in controllers
            }
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

        // Add code to check and add Brand column if missing
        try
        {
            // Note: No need to get logger again here, use the one from the outer scope
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            var connection = dbContext.Database.GetDbConnection();

            // Open connection if it's not already open
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                // Check if Brand column exists in SQLite
                command.CommandText = @"SELECT COUNT(*) FROM pragma_table_info('Products') WHERE name = 'Brand'";

                int columnExists = Convert.ToInt32(command.ExecuteScalar());
                if (columnExists == 0)
                {
                    // Add Brand column if it doesn't exist
                    using (var alterCommand = connection.CreateCommand())
                    {
                        alterCommand.CommandText = "ALTER TABLE Products ADD COLUMN Brand TEXT NULL";
                        alterCommand.ExecuteNonQuery();
                    }
                }
            }

            // Check and add AvatarUrl column to AspNetUsers
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT COUNT(*) FROM pragma_table_info('AspNetUsers') WHERE name = 'AvatarUrl'";

                int columnExists = Convert.ToInt32(command.ExecuteScalar());
                if (columnExists == 0)
                {
                    // Add AvatarUrl column if it doesn't exist
                    using (var alterCommand = connection.CreateCommand())
                    {
                        alterCommand.CommandText = "ALTER TABLE AspNetUsers ADD COLUMN AvatarUrl TEXT NULL";
                        alterCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Use the logger declared in the outer scope
            logger.LogError(ex, "An error occurred while checking/adding the Brand or AvatarUrl columns.");
        }
    }
    catch (Exception ex)
    {
        // Use the logger declared in the outer scope
        logger.LogError(ex, "An error occurred during database migration/initialization.");
        // Removed duplicate log message from here
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
    // First create cafe categories
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Check if we need to seed cafe categories
        if (!dbContext.Categories.Any(c => c.Name == "Hot Coffee" || c.Name == "Cold Coffee"))
        {
            var cafeCategories = new List<Category>
            {
                new Category { Name = "Hot Coffee" },
                new Category { Name = "Cold Coffee" },
                new Category { Name = "Tea" },
                new Category { Name = "Pastries" },
                new Category { Name = "Sandwiches" },
                new Category { Name = "Desserts" }
            };

            dbContext.Categories.AddRange(cafeCategories);
            dbContext.SaveChanges();
            Console.WriteLine("Cafe categories added to database!");
        }
    }

    // Then add Hong Tra product in development mode
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Check if Hong Tra already exists
        if (!context.Products.Any(p => p.Name == "Hong Tra Tea"))
        {
            // Add a new Hong Tra product
            var hongTra = new Product
            {
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
            };

            context.Products.Add(hongTra);
            context.SaveChanges();

            Console.WriteLine("Hong Tra Tea product added to database!");
        }

        // Add all other drinks from assets
        var newDrinks = new List<Product>();

        // Check if new drinks exist before adding
        if (!context.Products.Any(p => p.Name == "Traditional Vietnamese Coffee"))
        {
            newDrinks.Add(new Product {
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
            });

            newDrinks.Add(new Product {
                Name = "Black Coffee",
                Description = "Strong and aromatic black coffee made from freshly ground beans.",
                Price = 35000,
                CategoryId = 1, // Hot Coffee
                ImageUrl = "/images/coffee/CafeDen.jpg",
                IsAvailable = true,
                CanCustomize = false,
                HasSizeOptions = true,
                PrepTime = "2-3 min",
                IsFeatured = false
            });

            newDrinks.Add(new Product {
                Name = "Milk Coffee",
                Description = "Traditional coffee with condensed milk, a Vietnamese favorite.",
                Price = 40000,
                CategoryId = 1, // Hot Coffee
                ImageUrl = "/images/coffee/Cafe-Y.jpg",
                IsAvailable = true,
                CanCustomize = true,
                HasSizeOptions = true,
                PrepTime = "2-3 min",
                IsFeatured = true
            });

            newDrinks.Add(new Product {
                Name = "Cappuccino",
                Description = "Espresso with steamed milk foam, sprinkled with cocoa powder.",
                Price = 45000,
                CategoryId = 1, // Hot Coffee
                ImageUrl = "/images/coffee/Cappuchino.jpg",
                IsAvailable = true,
                CanCustomize = true,
                HasSizeOptions = true,
                PrepTime = "3-4 min",
                IsFeatured = false
            });

            newDrinks.Add(new Product {
                Name = "Nespresso Specialty",
                Description = "Premium coffee made with Nespresso capsules for a rich, aromatic experience.",
                Price = 50000,
                CategoryId = 1, // Hot Coffee
                ImageUrl = "/images/coffee/Nespresso.jpg",
                IsAvailable = true,
                CanCustomize = false,
                HasSizeOptions = true,
                PrepTime = "1-2 min",
                IsFeatured = true
            });

            newDrinks.Add(new Product {
                Name = "Iced Lemon Tea",
                Description = "Refreshing black tea with fresh lemon and ice.",
                Price = 38000,
                CategoryId = 3, // Tea
                ImageUrl = "/images/coffee/TraChanh.jpg",
                IsAvailable = true,
                CanCustomize = true,
                HasSizeOptions = true,
                PrepTime = "2-3 min",
                IsFeatured = false
            });

            newDrinks.Add(new Product {
                Name = "Black Tea",
                Description = "Premium black tea with a bold flavor, served hot.",
                Price = 35000,
                CategoryId = 3, // Tea
                ImageUrl = "/images/coffee/TraDen.jpg",
                IsAvailable = true,
                CanCustomize = false,
                HasSizeOptions = true,
                PrepTime = "2-3 min",
                IsFeatured = false
            });

            newDrinks.Add(new Product {
                Name = "Classic Cocktail",
                Description = "A refreshing non-alcoholic cocktail with mixed fruits and soda.",
                Price = 55000,
                CategoryId = 3, // Specialty Drinks
                ImageUrl = "/images/coffee/Cocktail.jpg",
                IsAvailable = true,
                CanCustomize = true,
                HasSizeOptions = true,
                PrepTime = "4-5 min",
                IsFeatured = true
            });

            newDrinks.Add(new Product {
                Name = "Premium Mixed Cocktails",
                Description = "A selection of tropical non-alcoholic cocktails with fresh fruits.",
                Price = 60000,
                CategoryId = 3, // Specialty Drinks
                ImageUrl = "/images/coffee/Cocktails.jpg",
                IsAvailable = true,
                CanCustomize = true,
                HasSizeOptions = true,
                PrepTime = "5-6 min",
                IsFeatured = true
            });

            // Add all products to database
            if (newDrinks.Count > 0)
            {
                context.Products.AddRange(newDrinks);
                context.SaveChanges();
                Console.WriteLine($"{newDrinks.Count} new drinks added to database!");
            }
        }
    }
}

app.UseSession();
app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Add redirect for Account routes to Identity
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    // Redirect /Account/Login to /Identity/Account/Login
    if (path.StartsWith("/Account/Login", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }

    // Redirect /Account/Logout to /Identity/Account/Logout
    if (path.StartsWith("/Account/Logout", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Identity/Account/Logout");
        return;
    }

    // Redirect /Account/Register to /Identity/Account/Register
    if (path.StartsWith("/Account/Register", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Identity/Account/Register");
        return;
    }

    await next();
});

app.MapRazorPages();

app.MapControllers();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map health endpoint
app.MapHealthChecks("/health");

app.Run();
