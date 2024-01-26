using Hotel_Reservation.Context;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
namespace Hotel_Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userobj)
        {

            if (userobj == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Username == userobj.Username && x.Password == userobj.Password);

            if (user == null)
            return NotFound(new { message = "Invalid credentials" });

            user.Token = CreateJwtToken(user);      
            
            return Ok(
                new {
                   token = user.Token,
                    message = "Login Success" });

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userobj)
        {
            if (userobj == null)
                return BadRequest();
           // userobj.Password=PasswordHasher.HashPassword(userobj.Password);
            userobj.Role = "user";
            userobj.Token = "";
            await _context.Users.AddAsync(userobj);
            await _context.SaveChangesAsync();
            return Ok(
                new {message=
                "Register Success"});
           
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok( await _context.Users.ToListAsync());
        }
        private string CreateJwtToken(User user)
        {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("uUbQeThWmZq4t7w!z%C*F-JaNcRfUjXn\r\n");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Role, user.Role),
                new Claim (ClaimTypes.Name,$"{user.Username}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,

            };
            var token=jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);


                }
       


    }


   




}
