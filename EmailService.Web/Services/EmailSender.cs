using EmailService.Web.Models.Dtos;
using EmailService.Web.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EmailService.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurationDto _emailConfiguration;

        public string RelativePath { get; set; }

        public EmailSender(EmailConfigurationDto emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
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
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, message.Sender));
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

        private async Task Send(string sender, string appKey, MimeMessage mailMessage)
        {
            await Task.Run(() =>
            {
                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
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
