using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EmailService.Web.Models.Contracts
{
    public class Message : IValidatableObject
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string AppKey { get; set; }
        [Required]
        public List<string> Recipients { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            foreach(string recipient in Recipients)
            {
                try
                {
                    new MailAddress(recipient);
                }
                catch(Exception)
                {
                    errors.Add(new ValidationResult($"{recipient} is not a valid Email address"));
                }
            }   

            return errors;
        }
    }
}
