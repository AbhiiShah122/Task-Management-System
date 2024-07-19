using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Model.Request.AuthenticationController;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJWTService _jwtService;

        public AuthenticationController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        //Imporvements: Instead of outputting 500 for invalid error code we can provide speficific errors with it's related code as well like 404.
        
        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var token = _jwtService.GenerateToken(loginModel.Email, loginModel.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the login request: {ex.Message}");
            }
        }
    }
}