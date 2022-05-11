using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SREZAPI.Models;

namespace SREZAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public static SREZITContext? DBContext;
        private IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
            DBContext = new SREZITContext();
        }

        [AllowAnonymous]
        [HttpGet ("Login")]
        public IActionResult Login(string email, string Password)
        {
            User? user = null;
            try
            {
                user = DBContext.Users.FirstOrDefault(c => c.Email == email && c.Password == Password);
            }
            catch (Exception ex)
            {
                return NotFound(404);
            }

            if (user != null)
            {
                string token = GenerateToken(user);

                return Ok(token);
            }
            return NotFound(405);
        }

        [AllowAnonymous]
        [HttpGet ("Registration")]
        public IActionResult Registration(string email, string Password, string FullName)
        {
            var user1 = DBContext.Users.FirstOrDefault(c => c.Email == email);
            if (user1 != null)
            {
                return Ok(201);
            }

            try
            {
                User user = new User();
                Client client = new Client();
                user.Email = email;
                user.Password = Password;
                client.FullName = FullName;
                client.UserId = user.UserId;

                DBContext.Users.Add(user);
                DBContext.Clients.Add(client);
                DBContext.SaveChanges();
                return Ok(200);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet ("PassRec")]
        public IActionResult PasswordRecovery(string email)
        {
            var user = DBContext.Users.FirstOrDefault(c => c.Email == email);
            if (user != null)
                return Ok(200);
            else
                return NotFound(100);
        }

        [AllowAnonymous]
        [HttpGet ("PassChg")]
        public IActionResult ChangePassword(string email, string password)
        {
            try
            {
                var User = DBContext.Users.FirstOrDefault(c => c.Email == email);
                if (User == null)
                {
                    return Ok(100);
                }
                User.Password = password;
                DBContext.SaveChanges();
                return Ok(200);
            } catch (Exception ex)
            {
                return BadRequest(100);
            }
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Client? client = DBContext.Clients.FirstOrDefault(c => c.UserId == user.UserId);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, client.FullName.Split(' ')[0]),
                new Claim(ClaimTypes.Surname, client.FullName.Split(' ')[1]),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
