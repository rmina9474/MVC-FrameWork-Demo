namespace Reina.MacCredy.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IEnumerable<Product> AllProducts { get; set; } = new List<Product>();
    }
}