using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get an entity Account by Login and Password.
        /// </summary>
        Account? GetByLoginAndPassword(string login, string hashedPassword);

        /// <summary>
        /// Save a link account creation in DB.
        /// </summary>
        LinkAccountCreation SaveLinkAccountCreation(LinkAccountCreation link);

        /// <summary>
        /// Get link account creation by mail.
        /// </summary>
        LinkAccountCreation? GetLinkAccountCreationByMail(string mail);

        /// <summary>
        /// Verify if new user has already an account.
        /// </summary>
        bool ExistsAccountByLogin(string login);

        /// <summary>
        /// Save account.
        /// </summary>
        Account SaveAccount(Account account);
    }
}
