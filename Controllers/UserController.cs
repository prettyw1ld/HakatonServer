using HakatonServer.Auth;
using HakatonServer.Data;
using HakatonServer.Dtos;
using HakatonServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static HakatonServer.Dtos.UserDto;

namespace HakatonServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        public static string HashPassword(string Password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(Password);
        }

        public static bool VerifyPassword(string Password, string Hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(Password, Hash);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthOptions>> UserLoginAsync([FromBody] UserLogin login)
        {
            try
            {
                
                var user = await _db.Users.Where(x => x.Email == login.Email).FirstAsync();

                if (user is null)
                {
                    return BadRequest("Неудачный запрос к серверу");
                }

                if (VerifyPassword(login.Password, user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.Email, user.Email)
                    };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MegaSuperSecret2006HakatonAVIATECH"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:7088/",
                        audience: "https://localhost:7088/",
                        claims: claims,
                        expires: DateTime.Now.AddDays(7),
                        signingCredentials: signinCredentials
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new AuthenticatedResponse { Token = tokenString });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("create")]
        public ActionResult<AuthOptions> UserCreateAsync([FromBody] UserCreate userCreate)
        {
            try
            {

                var user = new User()
                {
                    Name = userCreate.Name,
                    Lastname = userCreate.Lastname,
                    Password = HashPassword(userCreate.Password),
                    Email = userCreate.Email,
                };

                _db.Users.Add(user);
                _db.SaveChanges();
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MegaSuperSecret2006HakatonAVIATECH"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7088/",
                    audience: "https://sad:7088/",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: signinCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new AuthenticatedResponse { Token = tokenString });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
