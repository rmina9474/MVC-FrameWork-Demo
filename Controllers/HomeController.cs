using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Reina.MacCredy.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ApplicationDbContext context)
    {
        _logger = logger;
        _productRepository = productRepository;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();

            // Get featured products for the main carousel
            var featuredProducts = products.Where(p => p.IsFeatured).ToList();

            // Create the view model
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = featuredProducts,
                AllProducts = products
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error in Home/Index: {ex.Message}");

            // Return a view with empty data rather than showing an error
            return View(new HomeViewModel
            {
                FeaturedProducts = new List<Product>(),
                AllProducts = new List<Product>()
            });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Locations()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            ErrorMessage = "An error occurred while processing your request."
        });
    }

    public async Task<IActionResult> Menu(string searchQuery, string sortOrder)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            products = products.Where(p =>
                p.Name.ToLower().Contains(searchQuery) ||
                (p.Description != null && p.Description.ToLower().Contains(searchQuery)) ||
                (p.Category != null && p.Category.Name.ToLower().Contains(searchQuery))
            );

            ViewBag.SearchQuery = searchQuery;
        }

        // Apply sorting
        switch (sortOrder)
        {
            case "asc":
                products = products.OrderBy(p => p.Price);
                ViewBag.CurrentSort = "asc";
                break;
            case "desc":
                products = products.OrderByDescending(p => p.Price);
                ViewBag.CurrentSort = "desc";
                break;
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }

        return View(await products.ToListAsync());
    }
}
