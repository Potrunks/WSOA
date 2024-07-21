using WSOA.Shared.Dtos;
using WSOA.Shared.Resources;

namespace WSOA.Shared.ViewModel
{
    public class AccountViewModel
    {
        public AccountViewModel() { }

        public AccountViewModel(AccountDto account, string BaseUrl)
        {
            Login = account.Login;
            FirstName = account.FirstName;
            LastName = account.LastName;
            AccountCreationLink = string.Format(RouteResources.CREATE_ACCOUNT_PAGE, BaseUrl);
            AccountResetPwdLink = account.ForgotPasswordKey != null && account.ForgotPasswordExpirationDate != null && account.ForgotPasswordExpirationDate >= DateTime.UtcNow ? string.Format(RouteResources.RESET_PWD_PAGE, BaseUrl, account.Id, account.ForgotPasswordKey) : string.Empty;
        }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccountCreationLink { get; set; }

        public string AccountResetPwdLink { get; set; }
    }
}
