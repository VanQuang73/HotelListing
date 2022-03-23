using HotelListing.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ISendMailService _emailSender;

        // Các dịch vụ được Inject vào: UserManger, SignInManager, ILogger, IEmailSender
        public MailController(ISendMailService emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            await _emailSender.SendEmailAsync("quang.nv2742@gmail.com", "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách Bấm vào đây.");
            return Ok();
        }
    }
}
