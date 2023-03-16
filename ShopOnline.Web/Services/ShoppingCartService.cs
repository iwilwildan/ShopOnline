using Blazored.LocalStorage;
using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        public event Action<int> OnShoppingCartChanged;

        private readonly ILocalStorageService _localStorage;
        public ShoppingCartService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", itemToAdd);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    throw new Exception(msg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Enumerable.Empty<CartItemDto>();
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    throw new Exception(msg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartDto> GetCart(int userId)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (token is not null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
                var result = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetCart");
                if (result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartDto);
                    }
                    return await result.Content.ReadFromJsonAsync<CartDto>();
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
    }
}
