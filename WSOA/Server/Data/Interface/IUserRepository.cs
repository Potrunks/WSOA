using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by account ID.
        /// </summary>
        public User GetUserByAccountId(int accountId);
    }
}
