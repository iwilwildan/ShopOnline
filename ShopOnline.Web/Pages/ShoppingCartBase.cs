using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {

        public string ErrorMessage { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService{ get; set; }
        public IEnumerable<CartItemDto> ShoppingCartItems{ get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDeleted = await ShoppingCartService.DeleteItem(id);
            if (cartItemDeleted != null)
            {
                ShoppingCartItems = ShoppingCartItems.ToList().Where(x => x.Id != id);
            }
        }

        
    }
}
