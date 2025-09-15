using SmartReturns.Models;

namespace SmartReturns.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(int id);
    }
}
