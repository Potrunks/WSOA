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
    }
}
