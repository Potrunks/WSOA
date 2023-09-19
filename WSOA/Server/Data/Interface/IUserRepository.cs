using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by account ID.
        /// </summary>
        User GetUserByAccountId(int accountId);

        /// <summary>
        /// Verify if user already exists by mail.
        /// </summary>
        bool ExistsUserByMail(string mail);

        /// <summary>
        /// Get all existing profile in DB.
        /// </summary>
        IEnumerable<Profile> GetAllProfiles();

        /// <summary>
        /// Save user.
        /// </summary>
        User SaveUser(User user);

        /// <summary>
        /// Get all users in DB.
        /// </summary>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Get user by ID.
        /// </summary>
        User GetUserById(int usrId);
    }
}
