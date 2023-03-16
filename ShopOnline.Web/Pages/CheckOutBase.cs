using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class CheckOutBase:ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        public int UserID { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected int TotalQty { get; set; }
        protected string PaymentDesc { get; set; }
        protected decimal PaymentAmount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await AuthState;
                if (authState.User.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(authState.User.FindFirst("userId").Value);
                }
                ShoppingCartItems = await ShoppingCartService.GetItems(UserID);
                if (ShoppingCartItems != null)
                {
                    Guid orderGuid = new Guid();

                    PaymentAmount = ShoppingCartItems.Sum(i => i.Price);
                    TotalQty = ShoppingCartItems.Sum(i => i.Qty);
                    PaymentDesc = $"O_{UserID}_{orderGuid}";

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await JSRuntime.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
