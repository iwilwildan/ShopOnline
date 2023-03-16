using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public ProductService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
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
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>($"api/Product/{categoryId}/GetItemsByCategory");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var response = await _httpClient.GetAsync("api/Product/GetProductCategories");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code - {response.StatusCode} Message - {msg}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
