using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagement_WebAPI.Data.DTO;
using UserManagement_WebAPI.Data.Entities;
using UserManagement_WebAPI.Data.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement_WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly JWT _jwtConfig;

        public UserController(ILogger<UserController> logger,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IOptions<JWT> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("register-user")]
        public async Task<object> RegisterUser([FromBody] RegisterUser registerUser)
        {
            try
            {

                var user = new AppUser()
                {
                    UserName = registerUser.Email,
                    FullName = registerUser.FullName,
                    Email = registerUser.Email,
                    EmailConfirmed = true,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    return new ResponseModel(Data.Enums.ResponseCode.OK,"User has been registered",null);
                }
                return BadRequest(result.Errors);
            }
            catch(Exception ex)
            {
                return new ResponseModel(Data.Enums.ResponseCode.Error, ex.Message, null);
            }
        }

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-all-users")]
        public async Task<object> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.Select(x=>new UserDTO(x.FullName,x.Email,x.UserName,x.DateCreated));
                return Ok(users);
            }
            catch(Exception ex)
            {
                return new ResponseModel(Data.Enums.ResponseCode.Error, ex.Message, null);
            }
        }

        [HttpPost("login-user")]
        public async Task<object> LoginUser([FromBody] LoginVM loginVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByEmailAsync(loginVM.Email);
                        var user = new UserDTO(appUser.FullName, appUser.Email, appUser.UserName, appUser.DateCreated);
                        user.Token = GenerateToken(appUser);
                        return new ResponseModel(Data.Enums.ResponseCode.OK,"", user);
                    }
                }
                return BadRequest("User dont exists");
            }
            catch (Exception ex)
            {
                return new ResponseModel(Data.Enums.ResponseCode.Error, ex.Message, null);
            }
        }

        private string GenerateToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

    }
}