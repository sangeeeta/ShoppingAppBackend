using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Interface;
using ShoppingApp.Model;
using ShoppingApp.Model_Request;
using ShoppingApp.Model_Response;
using ShoppingApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingApp.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/<ShoppingController>
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<ProductModel> data = await _productService.GetProductsAsync();
                if (data == null || !data.Any())
                {
                    return NotFound(ResponseHandller.GetAppResponse(ResponseType.NotFound, null));
                }

                return Ok(ResponseHandller.GetAppResponse(ResponseType.Success, data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandller.GetExceptionResponse(ex));
            }
        }

        // GET api/<ShoppingController>/5
        [HttpGet("GetProductById /{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                ProductModel? data = await _productService.GetProductsByIdAsync(id);
                if (data == null)
                {
                    type = ResponseType.NotFound;
                }
                return Ok(ResponseHandller.GetAppResponse(type, data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandller.GetExceptionResponse(ex));
            }
        }


        [HttpPost("SaveUpdateProduct")]
        public async Task<IActionResult> SaveUpdateProduct(ProductModel product)
        {
            try
            {
                await _productService.SaveUpdateProductAsync(product); // return the saved/updated entity
                return Ok(new { code = 200, message = "Saved/Updated product successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 400, message = ex.Message });
            }
        }


        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductRequest request)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                await _productService.DeleteProductAsync(request.Id);
                return Ok(new { code = 200, message = "Deleted product successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandller.GetExceptionResponse(ex));
            }
        }

    }
}
