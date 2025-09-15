using Microsoft.EntityFrameworkCore;
using SmartReturns.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace SmartReturns.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _db;
        private readonly IDatabase _cache;

        public ProductService(ProductDbContext db, IConnectionMultiplexer redis)
        {
            _db = db;
            _cache = redis.GetDatabase();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            string cacheKey = $"product:{id}";

            // Try get from Redis
            var cachedProduct = await _cache.StringGetAsync(cacheKey);
            if (cachedProduct.HasValue)
            {
                return JsonSerializer.Deserialize<Product>(cachedProduct!);
            }

            // Fetch from DB
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                // Save to Redis with expiration
                await _cache.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(product),
                    TimeSpan.FromMinutes(5)
                );
            }

            return product;
        }
    }
}
