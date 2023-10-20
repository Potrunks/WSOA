using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly WSOADbContext _dbContext;

        public UserRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Profile> GetAllProfiles()
        {
            return _dbContext.Profiles;
        }

        public User GetUserByAccountId(int accountId)
        {
            return _dbContext.Users.Single(usr => usr.AccountId == accountId);
        }

        public bool ExistsUserByMail(string mail)
        {
            return _dbContext.Users.Any(u => u.Email == mail);
        }

        public User SaveUser(User user)
        {
            if (user.Id == 0)
            {
                _dbContext.Users.Add(user);
            }
            _dbContext.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAllUsers(IEnumerable<int>? blacklistUserIds = null)
        {
            blacklistUserIds = blacklistUserIds ?? new List<int>();
            return
                (
                    from usr in _dbContext.Users
                    where !blacklistUserIds.Contains(usr.Id)
                    select usr
                );
        }

        public User GetUserById(int usrId)
        {
            return _dbContext.Users.Single(usr => usr.Id == usrId);
        }

        public bool ExistsBusinessActionByProfileCode(string profileCode, string businessActionCode)
        {
            return
                (
                    from ba_by_prof in _dbContext.BusinessActionsByProfileCode
                    where ba_by_prof.ProfileCode == profileCode
                            && ba_by_prof.BusinessActionCode == businessActionCode
                    select ba_by_prof
                )
                .Any();
        }
    }
}
