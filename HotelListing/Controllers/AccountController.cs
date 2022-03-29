using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;

        public AccountController(ILogger<AccountController> logger,
            IAuthManager authManager)
        {
            _logger = logger;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            object result = await _authManager.Register(userDTO);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login Attempt for {userDTO.Email} ");
            var result = await _authManager.Login(userDTO);
            return Ok(result);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authManager.Logout();
            return Ok(result);
        }

        [HttpGet]
        [Route("ConfirmedEmail")]
        public async Task<IActionResult> ConfirmedEmail(Guid id, string code)
        {
            var result = await _authManager.ConfirmedEmail(id, code);
            return Ok(result);
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string mail)
        {
            var result = await _authManager.ForgotPassword(mail);
            return Ok(result);
        }

        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code, [FromBody] ResetPassword resetPassword)
        {
            var result = await _authManager.ResetPassword(code, resetPassword);
            return Ok(result);
        }
    }
}