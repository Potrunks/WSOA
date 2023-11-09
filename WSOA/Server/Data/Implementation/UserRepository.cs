using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;

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

        public User GetUserWinnerByTournamentId(int tournamentId)
        {
            return
                (
                    from user in _dbContext.Users
                    join player in _dbContext.Players on user.Id equals player.UserId
                    join tournament in _dbContext.Tournaments on player.PlayedTournamentId equals tournament.Id
                    where
                        tournament.Id == tournamentId
                        && player.CurrentTournamentPosition == 1
                    select user
                )
                .Single();
        }

        public User GetFirstRankUserBySeasonCode(string seasonCode)
        {
            return
                (
                    from user in _dbContext.Users
                    join player in _dbContext.Players on user.Id equals player.UserId
                    join tournament in _dbContext.Tournaments on player.PlayedTournamentId equals tournament.Id
                    where
                        tournament.Season == seasonCode
                        && player.PresenceStateCode == PresenceStateResources.PRESENT_CODE
                        && tournament.IsOver == true
                    group new { player } by user into grouped
                    select new
                    {
                        User = grouped.Key,
                        Points = grouped.Sum(gr => gr.player.TotalWinningsPoint)
                    }
                )
                .OrderByDescending(dto => dto.Points)
                .First()
                .User;
        }
    }
}
