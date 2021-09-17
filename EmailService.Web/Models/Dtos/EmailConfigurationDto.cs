﻿

namespace EmailService.Web.Models.Dtos
{
    public class EmailConfigurationDto
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string UrlHttpHandler { get; set; }
    }
}