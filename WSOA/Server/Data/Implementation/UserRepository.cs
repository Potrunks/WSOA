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
    }
}
