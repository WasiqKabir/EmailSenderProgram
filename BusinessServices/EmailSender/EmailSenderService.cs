using BusinessServices.Repositories;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.EmailSender
{
    public class EmailSenderService : IEmailSenderRepository
    {
        public async Task SendEmail(MailMessage mailMessage)
        {
            //Create a SmtpClient to our smtphost: yoursmtphost
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.SmtpHost);
            //Send mail
            smtp.Send(mailMessage);
        }
    }
}
