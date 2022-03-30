using AutoMapper;
using HotelListing.Data;
using HotelListing.Mail;
using HotelListing.Models;
using HotelListing.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApiUser _user;
        private readonly IMapper _mapper;
        private readonly ISendMailService _emailSender;

        public AuthManager(UserManager<ApiUser> userManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ISendMailService emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(
                jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim("LoginFor", "Web")
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration.GetSection("Jwt").GetSection("Key").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<Repsonse> Register(UserDTO userDTO)
        {
            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                List<string> error = new List<string>();
                foreach (var e in result.Errors)
                {
                    error.Add(e.Description);
                }
                return new Repsonse
                {
                    statusCode = "201",
                    message = "Đăng ký thất bại",
                    developerMessage = error,
                    data = null
                };
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = string.Format($"/api/Account/ConfirmedEmail?id={user.Id}&code={code}");

            await _emailSender.SendEmailAsync(userDTO.Email, "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");
            await _userManager.AddToRolesAsync(user, userDTO.Roles);

            return new Repsonse
            {
                statusCode = "200",
                message = "Đăng ký thành công",
                developerMessage = null,
                data = null
            };
        }

        public async Task<Repsonse> Login(LoginUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_user, userDTO.Password);
            if (_user != null && validPassword)
            {
                return new Repsonse
                {
                    statusCode = "200",
                    message = Resources.LOGIN_SUCCESS,
                    data = new
                    {
                        Token = await CreateToken()
                    }
                };
            }
            return new Repsonse
            {
                statusCode = "400",
                message = Resources.LOGIN_FAIL,
                developerMessage = new List<string> { "Tài khoản hoặc mật khẩu không chính xác." }
            };
        }

        public async Task<Repsonse> Logout()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            //Gets list of claims.
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == ClaimTypes.Name)
                .FirstOrDefault();

            var user = await _userManager.FindByNameAsync(usernameClaim.Value);
            var result = await _userManager.RemoveAuthenticationTokenAsync(user, "Web", "Access");
            if (result.Succeeded)
            {
                return new Repsonse
                {
                    statusCode = "200",
                    message = Resources.LOGOUT_SUCCESS
                };
            }
            return new Repsonse
            {
                statusCode = "400",
                message = Resources.LOGOUT_FAIL
            };
        }

        public async Task<Repsonse> ConfirmedEmail(Guid id, string key)
        {
            List<string> e = new List<string>();
            var user = await _userManager.FindByIdAsync(id.ToString());
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(key));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {

                return new Repsonse
                {
                    statusCode = "200",
                    message = Resources.COMFIRMED_SUCCESS
                };
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    e.Add(error.Description);
                }
                return new Repsonse
                {
                    statusCode = "400",
                    message = Resources.COMFIRMED_FAIL,
                    developerMessage = e
                };
            }
        }

        public async Task<Repsonse> ForgotPassword(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new Repsonse
                {
                    statusCode = "400",
                    message = Resources.ACCOUNT_FAIL
                };
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = string.Format($"/api/Account/resetPassword?code={code}");
            await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu",
                    $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>bấm vào đây</a>.");
            return new Repsonse
            {
                statusCode = "200",
                message = Resources.FORGOT_PASSWORD_SUCCESS
            };
        }

        public async Task<Repsonse> ResetPassword(string key, ResetPassword resetPassword)
        {
            if (key == null)
            {
                return new Repsonse
                {
                    statusCode = "400",
                    message = Resources.NOT_TOKEN
                };
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                // Không thấy user
                return new Repsonse
                {
                    statusCode = "400",
                    message = Resources.NOT_ACCOUNT
                };
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(key));
            var result = await _userManager.ResetPasswordAsync(user, code, resetPassword.Password);
            List<string> e = new List<string>();
            if (result.Succeeded)
            {
                return new Repsonse
                {
                    statusCode = "200",
                    message = Resources.RESETPASSWORD_SUCCESS
                };
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    e.Add(error.Description);
                }
            }
            return new Repsonse
            {
                statusCode = "400",
                message = Resources.RESETPASSWORD_FAIL,
                developerMessage = e
            };
        }
    }
}