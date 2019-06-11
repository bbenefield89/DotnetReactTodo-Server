using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace dotnet_react_todo.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetUsers()
        {
            return Ok(await _context.users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser([FromBody] User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = hashedPassword;
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Created("/api/user", user);
        }

        [HttpPost("login")]
        async public Task<IActionResult> LogIn([FromBody] User user)
        {
            object response = new { error = "Username or Password is incorrect" };
            try
            {
                User existingUser = await _context.users.Where(userFound => userFound.username == user.username).FirstAsync();
                bool isPasswordsMatch = BCrypt.Net.BCrypt.Verify(user.password, existingUser.password);
                if (isPasswordsMatch == true)
                {
                    response = existingUser;
                }
            }
            catch (Exception exception)
            {
                if (exception is SaltParseException)
                {
                    response = new { error = "Error trying to authenticate user.\nPlease try again at a later time." };
                }
            }
            return Ok(response);
        }

    }
}
