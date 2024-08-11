
using DataAccess.Context;
using System;
using System.Net.Mail;
using System.Net;

namespace Common.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly eShopContext _context;

        public EmailSender(eShopContext context)
        {
            _context = context;
        }
        public async Task<string> SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var from = _context.tblWebConfigurations.Where(x => x.ConfigKey == "From").FirstOrDefault().ConfigValue;
                var password = _context.tblWebConfigurations.Where(x => x.ConfigKey == "Password").FirstOrDefault().ConfigValue;
                int Port = Convert.ToInt32(_context.tblWebConfigurations.Where(x => x.ConfigKey == "Port").FirstOrDefault().ConfigValue);
                var SMTP = _context.tblWebConfigurations.Where(x => x.ConfigKey == "SMTP").FirstOrDefault().ConfigValue;

                if (string.IsNullOrEmpty(from)
                    || string.IsNullOrEmpty(password)
                    || string.IsNullOrEmpty(Port.ToString())
                    || string.IsNullOrEmpty(Port.ToString()))
                {
                    return ("The value can't be null.");
                }

                var client = new SmtpClient(SMTP, Port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(from,
                    "tyagwdnmxgrkkfky")
                };

                 await client.SendMailAsync(
                    new MailMessage(from,
                                    to: email,
                                    subject,
                                    body
                                    ));

                return "Email sent successfully.";

            }
            catch (Exception ex)
            {

                return $"Failed to send email. Error: {ex.Message}";
            }
        }
    }
}
