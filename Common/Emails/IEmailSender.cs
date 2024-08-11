using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Emails
{
    public interface IEmailSender
    {
        Task<string> SendEmailAsync(string email, string subject, string body);
    }
}
