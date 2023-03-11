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

        public async Task<ProductCategory> GetCategory(int id)
        {
            var result =await _db.ProductCategories.FindAsync(id);
            return result;
        }

        public async Task<Product> GetItem(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int categoryId)
        {
            return await _db.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        }
    }
}
