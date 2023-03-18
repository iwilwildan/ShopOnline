using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopOnlineDbContext _db;
        private readonly IPasswordHasher _passwordHasher;
        public UserRepository(ShopOnlineDbContext db, IPasswordHasher passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        private async Task<bool> IsUserExists(string username)
        {
            return await _db.Users.AnyAsync(x => x.UserName == username);
        }
        
        public async Task<User> SignUp(UserDto user)
        {
            if (await IsUserExists(user.UserName))
            {
                throw new Exception($"User with username: {user.UserName} already exists");
            }
            var userToAdd = new User();
            userToAdd.Email = user.Email;
            userToAdd.UserName = user.UserName;

            //hashing password
            userToAdd.Password = _passwordHasher.HashPassword(user.Password);

            var newUser = await _db.Users.AddAsync(userToAdd);

            await _db.SaveChangesAsync();

            return userToAdd;
        }

        public async Task<User> SignIn(UserDto user)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (dbUser == null)
            {
                throw new Exception($"User with username: {user.UserName} is not exists");
            }

            var verify = _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                throw new Exception($"Invalid credential");
            }

            return dbUser;
        }
    }
}
