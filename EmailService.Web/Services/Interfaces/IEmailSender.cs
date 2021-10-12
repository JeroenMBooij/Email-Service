using EmailService.Web.Models;
using EmailService.Web.Models.Dtos;
using System.Threading.Tasks;

namespace EmailService.Web.Services.Interfaces
{
    public interface IEmailSender
    {
        string RelativePath { get; set; }

        Task SendHtmlEmail(MessageDto message);
        Task SendTextEmail(MessageDto message);
    }
}