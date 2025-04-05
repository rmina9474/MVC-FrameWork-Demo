using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Reina.MacCredy.Models;
using Reina.MacCredy.Extensions;
using System.Linq;

namespace Reina.MacCredy.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<int> GetCartItemCountAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return Task.FromResult(0);
                }

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    return Task.FromResult(0);
                }

                var cart = httpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                int cartCount = cart?.Items.Sum(item => item.Quantity) ?? 0;

                return Task.FromResult(cartCount);
            }
            catch
            {
                // Return 0 if any exception occurs
                return Task.FromResult(0);
            }
        }
    }
}