using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Model;
using ShoppingApp.Model_Request;
using ShoppingApp.Model_Response;
using ShoppingApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingApp.Controllers
{
   
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _db;
        public ProductController(Data.ShoppingDbContext _dbContext)
        {
           _db = new ProductService(_dbContext);
        }

        // GET: api/<ShoppingController>
        [HttpGet]
        [Route("api/[controller]/GetProducts")]
        public IActionResult GetProducts()
        {
            ResponseType type = ResponseType.Success;
            try
            {
                IEnumerable<ProductModel> data = _db.GetProducts();
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
        [HttpGet]
        [Route("api/[controller]/GetProductById/{id}")]
        public IActionResult Get(int id)
        {
            ResponseType type = ResponseType.Success;
            try
            {
                ProductModel data = _db.GetProductsById(id);
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



        [HttpPost]
        [Route("api/[controller]/SaveUpdateProduct")]
        public IActionResult SaveUpdateProduct([FromBody] ProductModel model)
        {
            try
            {
                _db.SaveUpdateProduct(model); // return the saved/updated entity
                return Ok(new { code = 200, message = "Saved/Updated product successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 400, message = ex.Message });
            }
        }

        // DELETE api/<ShoppingController>/5
        [HttpDelete]
        [Route("api/[controller]/DeleteProduct")]
        public IActionResult DeleteProduct([FromBody] DeleteProductRequest request)       
        {
            try
            {
                ResponseType type = ResponseType.Success;
                _db.DeleteProduct(request.Id);
                return Ok(new { code = 200, message = "Deleted product successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandller.GetExceptionResponse(ex));
            }
        }
    }
}
