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
        private readonly User _user;
        public ShoppingCartRepository(ShopOnlineDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            var userNameContext = httpContextAccessor.HttpContext.User.Identity.Name;
            if (userNameContext != null)
            {
                _user = _db.Users.First(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
            }
            
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

        public async Task<CartItem> DeleteItem(int id)
        {
            try
            {
                var itemToDelete = await _db.CartItems.FindAsync(id);
                if (itemToDelete != null)
                {
                    _db.CartItems.Remove(itemToDelete);
                    await _db.SaveChangesAsync();
                }
                return itemToDelete;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _db.Carts
                         join cartItem in _db.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.UserId == (_user != null ? _user.Id : userId)
                          select new CartItem
                         {
                             Id = cartItem.Id,
                             CartId = cartItem.CartId,
                             Qty = cartItem.Qty,
                             ProductId = cartItem.ProductId,
                         }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto qtyUpdateItem)
        {
            try
            {
                var item = await _db.CartItems.FindAsync(id);
                if (item != null)
                {
                    item.Qty = qtyUpdateItem.Qty;
                    await _db.SaveChangesAsync();
                }

                return item;
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<Cart> GetCart(int userId)
        {
            return await(from cart in _db.Carts
                         where cart.UserId == userId
                         select cart
                         ).SingleOrDefaultAsync();
        }

        public async Task<Cart> AddCart(int userId)
        {
            try
            {
                var newCart = new Cart()
                {
                    UserId = userId
                };

                var result = await _db.Carts.AddAsync(newCart);
                _db.SaveChanges();
                return result.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
