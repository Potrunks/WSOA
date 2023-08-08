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
    }
}
