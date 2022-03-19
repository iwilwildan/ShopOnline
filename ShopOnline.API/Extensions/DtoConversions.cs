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
    }
}
