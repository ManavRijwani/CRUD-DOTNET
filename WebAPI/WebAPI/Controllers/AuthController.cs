using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.DataAcess;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserDataAccess _userDataAccess;
        public AuthController(UserDataAccess userDataAccess, IConfiguration configuration)
        {
            _userDataAccess = userDataAccess;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest model)
        {
            // Your logic to validate the username and password
            Temp tm =_userDataAccess.IsValidUser(model.Username, model.Password);
            if (tm!=null)
            {
                var tokenString = GenerateToken(model.Username,tm);
                return Ok(new { Token = tokenString});

            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest model) 
        {
            try
            {
                _userDataAccess.RegisterUser(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            try
            {
               var res = _userDataAccess.GetUser();
                return Ok(res);
            }
            catch (Exception ex) 
            {
                return NotFound();
            }
        }

        [HttpGet("CheckUser")]
        public async Task<ActionResult<bool>> CheckUsernameExists([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            var exists = await _userDataAccess.CheckUsernameExists(username);
            return Ok(exists);
        }
        private string GenerateToken(string username, Temp model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyzsuperSecretKey@345");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, model.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = "https://localhost:5050",
                Audience = "https://localhost:5050",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    _configuration["JWT:Issuer"],
            //    _configuration["JWT:Audience"],
            //    new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, username),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //    },
            //    expires: DateTime.UtcNow.AddMinutes(30),
            //    signingCredentials: credentials
            //);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //return tokenHandler.WriteToken(token);
        }

        [HttpGet("{id}")]
        public IActionResult userbyID(int id)
        {
            var std = _userDataAccess.GetUserbyID(id);
            if (std == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(std);
            }
        }





    }

}


