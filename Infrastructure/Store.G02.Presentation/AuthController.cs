using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstractions;
using Store.G02.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager serviceManager) : ControllerBase
    {
        // login

        [HttpPost("Login")] // POST: /api/auth/login
        public async Task<IActionResult> Login(LoginDto loginDto)
        { 
            var result = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(result);
        }

        // register

        [HttpPost("Register")] // POST: /api/auth/register
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(result);
        }
    }
}
