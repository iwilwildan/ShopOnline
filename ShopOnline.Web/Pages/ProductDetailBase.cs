using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailBase:ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ProductDto Product { get; set; }
        public int? CartId { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        public int UserID { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await AuthState;
                if (authState.User.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(authState.User.FindFirst("userId").Value);
                    var cart = await ShoppingCartService.GetCart(UserID);
                    CartId = cart.Id;
                }
                Product = await ProductService.GetItem(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task OnAddtoCart_Click(CartItemToAddDto cartItemtoAddDto)
        {
            try
            {
                if (CartId == null)
                {
                    NavigationManager.NavigateTo("/signin");
                }
                else
                {
                    var result = await ShoppingCartService.AddItem(cartItemtoAddDto);

                    NavigationManager.NavigateTo("/ShoppingCart");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
