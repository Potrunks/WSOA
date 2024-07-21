using WSOA.Shared.Entity;

namespace WSOA.Server.Business.Interface
{
    public interface IMailService
    {
        /// <summary>
        /// Send mail for account creation.
        /// </summary>
        void SendMailAccountCreationLink(string recipientMail, string baseUri);

        /// <summary>
        /// Send mails.
        /// </summary>
        void SendMails(IEnumerable<string> recipientMails, string subject, string body);

        /// <summary>
        /// Send the mail to invite the user to re-initialize his login data.
        /// </summary>
        void SendResetAccountLoginMail(string recipientMail, Account account, string baseUri);
    }
}
