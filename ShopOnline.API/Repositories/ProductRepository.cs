using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;

namespace ShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _db;

        public ProductRepository(ShopOnlineDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await _db.ProductCategories.ToListAsync();
        }

        public Task<ProductCategory> GetCategory(int id)
        {
            return _db.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public Task<Product> GetItem(int id)
        {
            return _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await _db.Products.ToListAsync();
        }
    }
}
