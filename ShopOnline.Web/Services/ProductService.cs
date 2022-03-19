using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Product/{id}");
                if (result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }
                    return await result.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var msg = await result.Content.ReadAsStringAsync();
                    throw new Exception(msg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
