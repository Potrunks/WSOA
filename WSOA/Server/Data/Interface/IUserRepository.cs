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
        IEnumerable<User> GetAllUsers(IEnumerable<int>? blacklistUserIds = null);

        /// <summary>
        /// Get user by ID.
        /// </summary>
        User GetUserById(int usrId);

        /// <summary>
        /// Check if exists business action with profile code wanted.
        /// </summary>
        bool ExistsBusinessActionByProfileCode(string profileCode, string businessActionCode);

        /// <summary>
        /// Get user win tournament given.
        /// </summary>
        User GetUserWinnerByTournamentId(int tournamentId);

        /// <summary>
        /// Get the actual first user in the season given.
        /// </summary>
        User GetFirstRankUserBySeasonCode(string seasonCode);

        /// <summary>
        /// Get users by selected Ids.
        /// </summary>
        IEnumerable<User> GetUsersByIds(IEnumerable<int> userIds);
    }
}
