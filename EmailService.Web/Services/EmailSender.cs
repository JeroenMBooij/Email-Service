using EmailService.Web.Logic;
using EmailService.Web.Models.Dtos;
using EmailService.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System.Threading.Tasks;

namespace EmailService.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurationDto _emailConfiguration;
        private readonly IWebHostEnvironment _env;

        public string RelativePath { get; set; }

        public EmailSender(EmailConfigurationDto emailConfiguration, IWebHostEnvironment env)
        {
            _emailConfiguration = emailConfiguration;
            _env = env;
        }
        public async Task SendProjectEmail(string[] to, string subject)
        {
            var content = new ProjectHTMLBuilder(_env.ContentRootPath, _emailConfiguration.UrlHttpHandler);
            content.BuildContent();

            var message = new MessageDto(to, subject, content.Content);
            await Send(CreateEmailHTMLMessage(message));
        }

        public async Task SendHtmlEmail(MessageDto message)
        {
            await Send(CreateEmailHTMLMessage(message));
        }

        public async Task SendTextEmail(MessageDto message)
        {
            await Send(CreateEmailTextMessage(message));
        }

        public async Task SendTestEmail(string[] to, string name, string subject)
        {
            var content = new TestHTMLBuilder(_env.ContentRootPath);
            content.SetName(name)
                .BuildContent();

            var message = new MessageDto(to, subject, content.Content);
            await Send(CreateEmailHTMLMessage(message));
        }

        private MimeMessage CreateEmailHTMLMessage(MessageDto message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.From));
            emailMessage.To.AddRange(message.Recipients);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Content;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private MimeMessage CreateEmailTextMessage(MessageDto message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.From));
            emailMessage.To.AddRange(message.Recipients);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            await Task.Run(() =>
            {
                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);

                        client.Send(mailMessage);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        client.Disconnect(true);
                        client.Dispose();
                    }
                }
            });
        }



    }
}
