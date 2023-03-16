using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Web.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient,
                           AuthenticationStateProvider authStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }
        public async Task<UserSignedDto> SignIn(UserDto user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserDto>("api/Authentication/signin", user);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserSignedDto>();
                    
                    await _localStorage.SetItemAsync("authToken", result.Token);

                    ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token); 
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                    return result;
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

        public async Task<UserSignedDto> SignUp(UserDto user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserDto>("api/Authentication/signup", user);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserSignedDto>();

                    await _localStorage.SetItemAsync("authToken", result.Token);

                    ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                    return result;
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

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
