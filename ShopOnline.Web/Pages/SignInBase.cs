using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class SignInBase:ComponentBase
    {
        public UserDto User = new UserDto();
        public bool ShowAuthError { get; set; }
        public string ErrorMessage { get; set; }
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected async Task ExecuteSignIn()
        {
            try
            {
                ShowAuthError = false;
                if (User.Email is null)
                {
                    User.Email = "";
                }
                var result = await UserService.SignIn(User);
                if (result != null)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    ShowAuthError=true;
                    ErrorMessage = "there was unexpected error";
                }
                
            }
            catch (Exception e)
            {
                ShowAuthError = true;
                ErrorMessage = e.Message; 
            }
        }
    }
}
