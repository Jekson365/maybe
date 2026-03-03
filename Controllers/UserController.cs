using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TappApi.Models;
using TappApi.Interfaces;
using TappApi.Repositories;
using TappApi.ViewModels;
using TappApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TappApi.Services.Spotify;

namespace TappApi.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;
        private readonly IConfiguration _config;
        public UserController(AppDbContext context, JwtService jwt, IConfiguration config)
        {
            _context = context;
            _jwt = jwt;
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .OrderByDescending(u => u.Id)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
        
            if (user == null)
                throw new KeyNotFoundException("user not found");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest("ID mismatch");

            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Password = user.Password;
            existing.Role = user.Role;
            existing.Token = user.Token;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || user.Password != dto.Password)
                return Unauthorized();

            var token = _jwt.GenerateToken(user);

            user.Token = token;
            await _context.SaveChangesAsync();

            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            return Ok(new { message = "Logged in" });
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchItemViewModel searchItem)
        {
            SpotifyService _spotifyService = new SpotifyService();  
            var result = await _spotifyService.SearchForItem(searchItem,_config["secrets:spotifyToken"]);
            return Ok(result);
        }
    }
}