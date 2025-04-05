using System.Threading.Tasks;

namespace Reina.MacCredy.Services
{
    public interface IShoppingCartService
    {
        Task<int> GetCartItemCountAsync(string username);
    }
}