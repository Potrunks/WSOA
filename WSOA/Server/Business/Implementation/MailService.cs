using log4net;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;

namespace WSOA.Server.Business.Implementation
{
    public class MailService : IMailService
    {
        private readonly ILog _log = LogManager.GetLogger(nameof(MailService));

        public void SendMailAccountCreationLink(string recipientMail, string baseUri)
        {
            try
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
            catch (Exception e)
            {
                _log.Error(e.Message);
                throw new WarningException(MailServiceResources.SEND_MAIL_ACCOUNT_CREATION_FAILED);
            }
        }

        public void SendMails(IEnumerable<string> recipientMails, string subject, string body)
        {
            try
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
            catch (Exception e)
            {
                _log.Error(e.Message);
                throw new WarningException(MailServiceResources.SEND_MAIL_FAILED);
            }
        }

        public void SendResetAccountLoginMail(string recipientMail, Account account, string baseUri)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(MailServiceResources.MAIL_LOGIN);
                message.To.Add(new MailAddress(recipientMail));
                message.Subject = "World Series of Antoine : Re-initialisation de vos identifiants";
                message.Body = $"Cliquez sur le lien suivant pour re-initialiser vos identifiants : {baseUri}account/{account.Id}/reinit/{account.ForgotPasswordKey!.Value}";

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = MailServiceResources.SMTP_HOST;
                smtpClient.Port = MailServiceResources.PORT;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(MailServiceResources.MAIL_LOGIN, MailServiceResources.PWD);
                smtpClient.EnableSsl = true;

                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                throw new WarningException(MailServiceResources.SEND_MAIL_RESET_PWD_FAILED);
            }
        }
    }
}
