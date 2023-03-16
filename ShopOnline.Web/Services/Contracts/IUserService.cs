using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IUserService
    {
        Task<UserSignedDto> SignUp(UserDto user);
        Task<UserSignedDto> SignIn(UserDto user);
        Task SignOut();
    }
}
