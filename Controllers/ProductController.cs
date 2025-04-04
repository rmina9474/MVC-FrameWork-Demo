using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Repositories;
using Reina.MacCredy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Reina.MacCredy.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
            _userManager = userManager;
        }
        
        // Hiển thị danh sách sản phẩm
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Browse));
        }
        
        // Browse products for all users
        [AllowAnonymous]
        public async Task<IActionResult> Browse(string searchQuery, string sortOrder)
        {
            var products = await _productRepository.GetAllAsync();

            // Lọc theo từ khóa tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sắp xếp theo giá
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "asc":
                    products = products.OrderBy(p => p.Price).ToList(); // Giá tăng dần
                    break;
                case "desc":
                    products = products.OrderByDescending(p => p.Price).ToList(); // Giá giảm dần
                    break;
            }

            ViewBag.SearchQuery = searchQuery;
            return View(products);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ActionName("AdminIndex")]
        public async Task<IActionResult> AdminIndex(string searchQuery, string sortOrder)
        {
            var products = await _productRepository.GetAllAsync();

            // Lọc theo từ khóa tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sắp xếp theo giá
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "asc":
                    products = products.OrderBy(p => p.Price).ToList(); // Giá tăng dần
                    break;
                case "desc":
                    products = products.OrderByDescending(p => p.Price).ToList(); // Giá giảm dần
                    break;
            }

            ViewBag.SearchQuery = searchQuery;
            return View("Index", products);
        }

        // Hiển thị form thêm sản phẩm mới
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        
        // Xử lý thêm sản phẩm mới
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
       
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); //Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        
        // Allow all users to view product details
        [AllowAnonymous]
        public async Task<IActionResult> Display(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
                    
                if (product == null)
                {
                    return NotFound();
                }
                
                // Try to include reviews if the table exists
                try
                {
                    product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Reviews)
                        .FirstOrDefaultAsync(p => p.Id == id);
                        
                    // Load user information for each review separately to avoid nullability issues
                    if (product?.Reviews != null)
                    {
                        foreach (var review in product.Reviews)
                        {
                            await _context.Entry(review)
                                .Reference(r => r.User)
                                .LoadAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // If there's an error loading reviews (e.g., table doesn't exist yet),
                    // just continue with the product without reviews
                    Console.WriteLine($"Error loading reviews: {ex.Message}");
                }
                
                // Check if the current user has purchased this product
                bool hasOrdered = false;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    hasOrdered = await _context.OrderDetails
                        .Include(od => od.Order)
                        .AnyAsync(od => od.Order.UserId == userId && od.ProductId == id);
                }
                
                ViewBag.HasOrdered = hasOrdered;
                
                return View(product);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Display method: {ex.Message}");
                // Return a friendly error view
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "There was an error accessing the product. The database might be updating."
                });
            }
        }
        
        // Handle product review submission
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                ModelState.AddModelError("Rating", "Rating must be between 1 and 5");
            }
            
            if (string.IsNullOrWhiteSpace(comment))
            {
                ModelState.AddModelError("Comment", "Please provide a comment");
            }
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Display), new { id = productId });
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var product = await _context.Products.FindAsync(productId);
            
            if (user == null || product == null)
            {
                return NotFound();
            }
            
            // Check if user has already reviewed this product
            var existingReview = await _context.ProductReviews
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
                
            if (existingReview != null)
            {
                // Update existing review
                existingReview.Rating = rating;
                existingReview.Comment = comment;
                existingReview.CreatedAt = DateTime.Now;
                
                _context.ProductReviews.Update(existingReview);
            }
            else
            {
                // Create new review
                var review = new ProductReview
                {
                    ProductId = productId,
                    UserId = userId,
                    Rating = rating,
                    Comment = comment,
                    CreatedAt = DateTime.Now,
                    User = user,
                    Product = product
                };
                
                _context.ProductReviews.Add(review);
            }
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Display), new { id = productId });
        }
        
        // Hiển thị form cập nhật sản phẩm
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name",
            product.CategoryId);
            return View(product);
        }
        
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(int id, Product product,IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await
                _productRepository.GetByIdAsync(id);
                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới

                    product.ImageUrl = await SaveImage(imageUrl);

                }
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        
        // Hiển thị form xác nhận xóa sản phẩm
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        
        // Xử lý xóa sản phẩm
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
        // Delete a review (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId, int productId)
        {
            var review = await _context.ProductReviews.FindAsync(reviewId);
            
            if (review != null)
            {
                _context.ProductReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Display), new { id = productId });
        }
    }
}