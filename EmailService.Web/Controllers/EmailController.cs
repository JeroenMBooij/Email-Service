using EmailService.Web.Models.Contracts;
using EmailService.Web.Models.Dtos;
using EmailService.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmailService.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("Html")]
        public async Task SendHtmlEmail([FromBody] Message message)
        {
            await _emailSender.SendHtmlEmail(new MessageDto(message));
        }

        [HttpPost]
        [Route("Text")]
        public async Task SendTextEmail([FromBody] Message message)
        {
            await _emailSender.SendTextEmail(new MessageDto(message));
        }
    }
}
