using EmailService.Web.Models.Dtos;
using EmailService.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EmailService.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public string RelativePath { get; set; }

        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }



        public async Task SendHtmlEmail(MessageDto message)
        {
            await Send(message.Sender, message.AppKey, CreateEmailHTMLMessage(message));
        }

        public async Task SendTextEmail(MessageDto message)
        {
            await Send(message.Sender, message.AppKey, CreateEmailTextMessage(message));
        }

        private MimeMessage CreateEmailHTMLMessage(MessageDto message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", message.Sender));
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
            emailMessage.From.Add(new MailboxAddress("", message.Sender));
            emailMessage.To.AddRange(message.Recipients);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(string sender, string appKey, MimeMessage mailMessage)
        {
            await Task.Run(() =>
            {
                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.Connect(_config["EmailConfiguration:SmtpServer"], int.Parse(_config["EmailConfiguration:Port"]), true);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(sender, appKey);

                        client.Send(mailMessage);
                    }
                    catch(Exception error)
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
