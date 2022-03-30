using HotelListing.Models;
using System;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public interface IAuthManager
    {
        Task<Repsonse> Login(LoginUserDTO userDTO);
        Task<string> CreateToken();
        Task<Repsonse> Logout();
        Task<Repsonse> Register(UserDTO userDTO);
        Task<Repsonse> ConfirmedEmail(Guid id, string code);
        Task<Repsonse> ForgotPassword(string mail);
        Task<Repsonse> ResetPassword(string code, ResetPassword resetPassword);
    }
}