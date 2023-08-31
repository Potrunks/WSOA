using WSOA.Shared.Entity;

namespace WSOA.Server.Business.Interface
{
    public interface IMailService
    {
        /// <summary>
        /// Send mail for account creation.
        /// </summary>
        void SendMailAccountCreation(LinkAccountCreation link, string baseUri);
    }
}
