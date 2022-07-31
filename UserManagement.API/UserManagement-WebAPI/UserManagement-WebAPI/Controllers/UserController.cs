using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(ILogger<UserController> logger,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser registerUser)
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
                    return StatusCode(201);
                }
                return BadRequest(result.Errors);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.Select(x=>new UserDTO(x.FullName,x.Email,x.UserName,x.DateCreated));
                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginVM loginVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest("User dont exists");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}