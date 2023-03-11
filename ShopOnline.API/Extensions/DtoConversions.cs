using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
        {
            return (from product in products
                    join category in productCategories
                    on product.CategoryId equals category.Id
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        Price = product.Price,
                        Qty = product.Qty,
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product, ProductCategory productCategory)
        {
            return new ProductDto() { 
                Id = product.Id,
                Description = product.Description,
                ImageURL= product.ImageURL,
                CategoryName= productCategory.Name,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = productCategory.Id, 
                Name = product.Name, };
        }

        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, ProductCategory productCategory)
        {
            return (from product in products
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        CategoryId = productCategory.Id,
                        CategoryName = productCategory.Name,
                        Price = product.Price,
                        Qty = product.Qty,
                    }).ToList();
        }
        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        CartId = cartItem.CartId,
                        Id = cartItem.Id,
                        Price = product.Price,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Qty = cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty,

                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto() 
            {
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Id = cartItem.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty,

            
            };
        }

        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCSS = productCategory.IconCSS,
                    }).ToList();
        }
    }
}
