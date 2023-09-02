using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;

namespace WSOA.Server.Business.Implementation
{
    public class MailService : IMailService
    {
        public void SendMailAccountCreationLink(string recipientMail, string baseUri)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(MailServiceResources.MAIL_LOGIN);
            message.To.Add(new MailAddress(recipientMail));
            message.Subject = AccountBusinessResources.LINK_ACCOUNT_CREATION_MAIL_SUBJECT;
            message.Body = baseUri + RouteBusinessResources.ACCOUNT_CREATION;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = MailServiceResources.SMTP_HOST;
            smtpClient.Port = MailServiceResources.PORT;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(MailServiceResources.MAIL_LOGIN, MailServiceResources.PWD);
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }

        public void SendMails(IEnumerable<string> recipientMails, string subject, string body)
        {
            if (recipientMails.IsNullOrEmpty() || subject.IsNullOrEmpty() || body.IsNullOrEmpty())
            {
                throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, string.Join(" ou ", nameof(recipientMails), nameof(subject), nameof(body)), nameof(SendMails)));
            }
            MailMessage message = new MailMessage();
            message.From = new MailAddress(MailServiceResources.MAIL_LOGIN);
            foreach (string recipientMail in recipientMails)
            {
                message.To.Add(new MailAddress(recipientMail));
            }
            message.Subject = subject;
            message.Body = body;
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
