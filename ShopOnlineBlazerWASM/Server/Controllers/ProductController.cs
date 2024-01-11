using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineBlazerWASM.Server.Repositories;
using ShopOnlineBlazerWASM.Shared;
using ShopOnlineBlazerWASM.Shared.Dtos;
using ShopOnlineBlazerWASM.Server.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ShopOnlineBlazerWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductController(IProductRepository repo)
        {
            _repo = repo; 
        }
        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _repo.GetItems();
                var productCategories =await _repo.GetCategories();

                if(products==null||productCategories==null)
                {
                    return NotFound();
                }
                else
                {
                    var productDto = products.ConvertToDto(productCategories);
                    return Ok(productDto);

                }
            }
            catch (Exception)
            {


                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error occured while retrieving data from database");
            }

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _repo.GetItem(id);
               

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var productCategory = await _repo.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productCategory);
                    return Ok(productDto);

                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error occured while retrieving data from database");
            }

        }
        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _repo.GetCategories();

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }

        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _repo.GetCategory(categoryId);


                var productDtos = products.ConvertToDto();

                return Ok(productDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

    }
}

