using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        public int UserID { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await AuthState;
                if (authState.User.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(authState.User.FindFirst("userId").Value);
                }
                Products = await ProductService.GetItems();
                var shoppingCartItems = await ShoppingCartService.GetItems(UserID);
                var totalQty = shoppingCartItems.Sum(x => x.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }
    }
}
