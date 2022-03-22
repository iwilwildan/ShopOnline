using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public string ErrorMessage { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService{ get; set; }
        public IEnumerable<CartItemDto> ShoppingCartItems{ get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateCartSummary();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task UpdateQty_Input(int id)
        {
            await JSRuntime.InvokeVoidAsync("ChangeUpdateBtnVisibility", id, true);
        }
        private void UpdateTotalPriceItem(CartItemDto cartItemDto)
        {
            var item = ShoppingCartItems.FirstOrDefault(x => x.Id == cartItemDto.Id);
            if (item != null)
                item.TotalPrice = item.Price * item.Qty;
        }
        private void CalculateCartSummary()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDeleted = await ShoppingCartService.DeleteItem(id);
            if (cartItemDeleted != null)
            {
                ShoppingCartItems = ShoppingCartItems.ToList().Where(x => x.Id != id);
                CalculateCartSummary();
            }
        }

        protected async Task UpdateQty_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updatedItem = await ShoppingCartService.UpdateQty(new CartItemQtyUpdateDto { CartItemId = id, Qty = qty });
                    UpdateTotalPriceItem(updatedItem);
                    CalculateCartSummary();
                    await JSRuntime.InvokeVoidAsync("ChangeUpdateBtnVisibility", id, false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}
