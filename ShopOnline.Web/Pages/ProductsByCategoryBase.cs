using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsByCategoryBase:ComponentBase
    {
        [Parameter]
        public int CategoryID { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        public int UserID { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public string CategoryName { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                var authState = await AuthState;
                if (authState.User.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(authState.User.FindFirst("userId").Value);
                }
                Products = await ProductService.GetItemsByCategory(CategoryID);
                var shoppingCartItems = await ShoppingCartService.GetItems(UserID);
                var totalQty = shoppingCartItems.Sum(x => x.Qty);
                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
                
                if (Products.Count() > 0)
                {
                    CategoryName = Products.FirstOrDefault().CategoryName;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
