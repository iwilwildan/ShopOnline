using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItem(CartItemToAddDto item);
        Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto qtyUpdateItem);
        Task<CartItem> DeleteItem(int id);
        Task<IEnumerable<CartItem>> GetItems(int userId);
        Task<CartItem> GetItem(int id);
        Task<Cart> GetCart(int userId);
        Task<Cart> AddCart(int userId);
    }
}
