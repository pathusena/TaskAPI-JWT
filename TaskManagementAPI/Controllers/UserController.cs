using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using System.Security.Cryptography;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Authentications;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IAuthenticate _authenticate;
        public UserController(ApplicationDBContext context, IAuthenticate authenticate) { 
            _context = context;
            _authenticate = authenticate;
        }

        //POST: api/User
        [HttpPost]
        public async Task<IActionResult> Register(UserModel user) {
            try {
                if (user == null || string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest("Invalid User Data");
                }
                user.PasswordHash = _authenticate.HashPassword(user.Password);
                user.Active = true;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User Registered Successfully!");
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2601)
                {
                    return Conflict("Username or email is already in use. Please choose a different username or email.");
                }
                else
                {
                    // Handle other database-related errors
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
