using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Data.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WSOADbContext _dbContext;

        public AccountRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account? GetByLoginAndPassword(SignInFormViewModel signInFormVM)
        {
            return (
                from account in _dbContext.Accounts
                where account.Login == signInFormVM.Login
                        && account.Password == signInFormVM.Password
                select account
                )
                .SingleOrDefault();
        }

        public LinkAccountCreation? GetLinkAccountCreationByMail(string mail)
        {
            return _dbContext.LinkAccountCreations.SingleOrDefault(l => l.RecipientMail == mail);
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
    }
}
