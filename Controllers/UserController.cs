using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SnackVote_Backend.DTO;
using SnackVote_Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace SnackVote_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;


        public UserController(DataContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("getme"),Authorize]
        public ActionResult<object> GetMe()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var voteStatus = _context.Users.FirstOrDefault(x => x.UserName == userName);
            return Ok(new 
            { 
                voteStatus.HasVoted,
                userName,
                userRole
            });
        }


        [HttpPost("register"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            CreatePasswordHash(request.Password,out byte[] passwordHash, out byte[] passwordSalt);
            _ = new User();
            User? user = new User()
            {
                UserName = request.Username,
                UserRole = request.Role,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
            
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            var dbuser = await _context.Users.FirstOrDefaultAsync(c => c.UserName == request.Username && c.UserRole == request.Role );
            
            if(dbuser == null)
            {
                return BadRequest("User Not Found"); 
            }
            if (!VerifyPasswordHash(request.Password, dbuser.PasswordHash, dbuser.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(dbuser);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole)

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            

            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
