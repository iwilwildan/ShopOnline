using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> SignUp(UserDto user);
        Task<User> SignIn(UserDto user);
    }
}
