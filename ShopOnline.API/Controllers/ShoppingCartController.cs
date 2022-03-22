using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository) 
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<IActionResult> GetItems(int userId)
        {
            try
            {
                var result = await _shoppingCartRepository.GetItems(userId);
                if (result == null)
                {
                    return NoContent();
                }

                var products = await _productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("no products");
                }

                return Ok(result.ConvertToDto(products));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var result = await _shoppingCartRepository.GetItem(id);
                if (result == null)
                {
                    return NotFound();
                }

                var product = await _productRepository.GetItem(result.ProductId);
                if (product == null)
                {
                    throw new Exception("Product is missing");
                }

                return Ok(result.ConvertToDto(product));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] CartItemToAddDto cartItemToAdd)
        {
            try
            {
                var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAdd);
                if (newCartItem == null)
                {
                    return NoContent();

                }
                var product = await _productRepository.GetItem(cartItemToAdd.ProductId);
                if (product == null)
                {
                    throw new Exception("Product is missing");
                }

                var result = newCartItem.ConvertToDto(product);
                return CreatedAtAction(nameof(GetItem), new {id = result.Id}, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var itemToDelete = await _shoppingCartRepository.DeleteItem(id);
                if (itemToDelete == null)
                {
                    return NotFound();
                }

                var product = await _productRepository.GetItem(itemToDelete.ProductId);
                if (product == null)
                {
                    throw new Exception("Product is missing");
                }
                return Ok(itemToDelete.ConvertToDto(product));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateItemQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var updatedItem = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (updatedItem == null)
                {
                    return NotFound();
                }
                var product = await _productRepository.GetItem(updatedItem.ProductId);
                if (product == null)
                {
                    throw new Exception("Product is missing");
                }
                return Ok(updatedItem.ConvertToDto(product));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
