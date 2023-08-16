using System.Net;
using System.Net.Mail;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Shared.Entity;

namespace WSOA.Server.Business.Implementation
{
    public class MailService : IMailService
    {
        public void SendMailAccountCreation(LinkAccountCreation link)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(MailServiceResources.MAIL_LOGIN);
            message.To.Add(new MailAddress(link.RecipientMail));
            message.Subject = AccountBusinessResources.LINK_ACCOUNT_CREATION_MAIL_SUBJECT;
            message.Body = "https://localhost:7235" + string.Format(RouteBusinessResources.ACCOUNT_CREATION, link.Id);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = MailServiceResources.SMTP_HOST;
            smtpClient.Port = MailServiceResources.PORT;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(MailServiceResources.MAIL_LOGIN, MailServiceResources.PWD);
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }
    }
}
