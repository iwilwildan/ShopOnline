using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.API.Extensions;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var products = await _productRepository.GetItems();
                var productCategories = await _productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var product = await _productRepository.GetItem(id);
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await _productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productCategory);
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }
        }
        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<IActionResult> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetCategories();
                if (productCategories == null)
                {
                    return NoContent();
                }
                else
                {
                    var productCategoriesDto = productCategories.ConvertToDto();
                    return Ok(productCategoriesDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }
        }

        [HttpGet()]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<IActionResult> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetItemsByCategory(categoryId);
                var productCategory = await _productRepository.GetCategory(categoryId);

                if (products == null || productCategory == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategory);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }
        }
    }
}
