using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlServerOptionsAction: sqlOptions => 
        {
            sqlOptions.EnableRetryOnFailure();
            sqlOptions.CommandTimeout(30);
            // Allow multiple active result sets to avoid "There is already an open DataReader" error
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));

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

// Add HttpContextAccessor for permission checks
builder.Services.AddHttpContextAccessor();

// Add health checks
builder.Services.AddHealthChecks();

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
        // Apply migrations at startup
        context.Database.Migrate();
        logger.LogInformation("Database migration completed successfully.");

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
                // Check if Brand column exists
                command.CommandText = @"
                    IF NOT EXISTS (
                        SELECT 1
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'Brand'
                    )
                    BEGIN
                        ALTER TABLE Products ADD Brand nvarchar(max) NULL;
                    END";
                
                command.ExecuteNonQuery();
            }
            
            // Check and add AvatarUrl column to AspNetUsers
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    IF NOT EXISTS (
                        SELECT 1
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'AvatarUrl'
                    )
                    BEGIN
                        ALTER TABLE AspNetUsers ADD AvatarUrl nvarchar(max) NULL;
                    END";
                command.ExecuteNonQuery();
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
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

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
