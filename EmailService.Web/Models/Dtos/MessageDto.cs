using EmailService.Web.Models.Contracts;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace EmailService.Web.Models.Dtos
{
    public class MessageDto
    {
        public List<MailboxAddress> Recipients { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }

        public MessageDto(IEnumerable<string> to, string subject, string content)
        {
            Recipients = new List<MailboxAddress>();

            Recipients.AddRange(to.Select(x => new MailboxAddress("test", x)));
            Subject = subject;
            Content = content;
        }

        public MessageDto(Message message)
        {
            Recipients = new List<MailboxAddress>();

            Recipients.AddRange(message.Recipients.Select(x => new MailboxAddress("test", x)));
            Subject = message.Subject;
            Content = message.Content;
        }

    }
}
