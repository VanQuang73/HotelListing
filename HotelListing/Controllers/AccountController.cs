using AutoMapper;
using HotelListing.Data;
using HotelListing.Mail;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly ISendMailService _emailSender;

        public AccountController(UserManager<ApiUser> userManager,
            ILogger<AccountController> logger,
            IMapper mapper,
            IAuthManager authManager,
            ISendMailService emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
            _emailSender = emailSender;
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

            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = string.Format($"/api/Account/ConfirmedEmail?id={user.Id}&code={code}");

            await _emailSender.SendEmailAsync(userDTO.Email, "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");
            await _userManager.AddToRolesAsync(user, userDTO.Roles);
            return Accepted();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login Attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _authManager.ValidateUser(userDTO))
            {
                return Unauthorized();
            }

            return Accepted(new { Token = await _authManager.CreateToken() });
        }

        [HttpGet]
        [Route("ConfirmedEmail")]
        public async Task<IActionResult> ConfirmedEmail(Guid id, string code)
        {
            var _error = "";
            var user = await _userManager.FindByIdAsync(id.ToString());
            var _code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, _code);
            if (result.Succeeded)
            {

                return Ok("Xác thực thành công.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _error = error.Description;
                }
                return BadRequest(_error);
            }
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = string.Format($"/api/Account/resetPassword?code={code}");
            await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu",
                    $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>bấm vào đây</a>.");
            return Ok("Vào mail để lấy link reset password");
        }

        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code, [FromBody] ResetPassword resetPassword)
        {
            if (code == null)
            {
                return BadRequest("Mã token không có.");
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                // Không thấy user
                return BadRequest("Không có tài khoản nào.");
            }
            var _code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, _code, resetPassword.Password);
            var _error = "";
            if (result.Succeeded)
            {
                return Ok("Thay đổi mật khẩu thành công");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _error = error.Description;
                }
            }
            return BadRequest(_error);
        }
    }
}