using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Data;
using ShoppingApp.Model_Request;
using ShoppingApp.Model_Response;
using ShoppingApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShoppingDbContext _context;
        private readonly JwtService _jwt;

        public UserController(ShoppingDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestPoco request)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.UserId == request.UserId && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                Role = user.Role
            });
        }
    }
}
