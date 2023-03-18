using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.API.Repositories.Utilities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IShoppingCartRepository _shopCartRepository;

        public AuthenticationController(IUserRepository userRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _userRepository = userRepository;
            _shopCartRepository = shoppingCartRepository;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]UserDto userDto)
        {
            try
            {
                var newUser = await _userRepository.SignUp(userDto);
                await _shopCartRepository.AddCart(newUser.Id);

                var token = JwtGenerator.GenerateUserToken(newUser.UserName, newUser.Id.ToString());

                var result = newUser.ConvertToDto(token);
                return CreatedAtAction(nameof(SignUp), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserDto userDto)
        {
            try
            {
                var user = await _userRepository.SignIn(userDto);

                var token = JwtGenerator.GenerateUserToken(user.UserName, user.Id.ToString());

                var result = user.ConvertToDto(token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
        }
    }
}
