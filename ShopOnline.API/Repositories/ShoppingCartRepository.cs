using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _db;
        public ShoppingCartRepository(ShopOnlineDbContext db)
        {
            _db = db;
        }
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _db.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto item)
        {
            if (!await CartItemExists(item.CartId, item.ProductId))
            {

                var itemToAdd = await (from product in _db.Products
                                where product.Id == item.ProductId
                                select new CartItem{ 
                                    CartId = item.CartId,
                                    ProductId = item.ProductId,
                                    Qty = item.Qty,

                                }).SingleOrDefaultAsync();
                if (itemToAdd != null)
                {
                    var result = await _db.CartItems.AddAsync(itemToAdd);
                    await _db.SaveChangesAsync();
                    return result.Entity;
                }
            }   

            
            return null;
        }

        public Task<CartItem> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _db.Carts
                         join cartItem in _db.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.Id == userId
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             CartId = cartItem.CartId,
                             Qty = cartItem.Qty,
                             ProductId = cartItem.ProductId,
                         }).ToListAsync();
        }

        public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto qtyUpdateItem)
        {
            throw new NotImplementedException();
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _db.Carts
                          join cartItem in _db.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              CartId = cartItem.CartId,
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                          }).SingleOrDefaultAsync();
        }
    }
}
