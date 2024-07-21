using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WSOADbContext _dbContext;

        public AccountRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account? GetByLoginAndPassword(string login, string hashedPassword)
        {
            return (
                from account in _dbContext.Accounts
                where account.Login == login
                        && account.Password == hashedPassword
                select account
                )
                .SingleOrDefault();
        }

        public LinkAccountCreation? GetLinkAccountCreationByMail(string mail)
        {
            return _dbContext.LinkAccountCreations.SingleOrDefault(l => l.RecipientMail == mail);
        }

        public bool ExistsAccountByLogin(string login)
        {
            return _dbContext.Accounts.Any(a => a.Login == login);
        }

        public Account SaveAccount(Account account)
        {
            if (account.Id == 0)
            {
                _dbContext.Accounts.Add(account);
            }
            _dbContext.SaveChanges();
            return account;
        }

        public LinkAccountCreation SaveLinkAccountCreation(LinkAccountCreation link)
        {
            if (link.Id == 0)
            {
                _dbContext.LinkAccountCreations.Add(link);
            }
            _dbContext.SaveChanges();
            return link;
        }

        public void DeleteLinkAccountCreation(LinkAccountCreation link)
        {
            _dbContext.LinkAccountCreations.Remove(link);
            _dbContext.SaveChanges();
        }

        public IQueryable<Account> GetAccountsByMails(IEnumerable<string> mails)
        {
            return (
                    from acc in _dbContext.Accounts
                    join usr in _dbContext.Users on acc.Id equals usr.AccountId
                    where mails.Contains(usr.Email)
                    select acc
                );
        }

        public IQueryable<Account> GetAccountsByIds(IEnumerable<int> ids)
        {
            return (
                    from acc in _dbContext.Accounts
                    where ids.Contains(acc.Id)
                    select acc
                );
        }

        public IQueryable<AccountDto> GetAllAccountDtos()
        {
            DateTime now = DateTime.UtcNow;

            return (
                    from acc in _dbContext.Accounts
                    join usr in _dbContext.Users on acc.Id equals usr.AccountId
                    select new AccountDto
                    {
                        Id = acc.Id,
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        ForgotPasswordExpirationDate = acc.ForgotPasswordExpirationDate,
                        ForgotPasswordKey = acc.ForgotPasswordKey,
                        Login = acc.Login
                    }
                );
        }
    }
}
