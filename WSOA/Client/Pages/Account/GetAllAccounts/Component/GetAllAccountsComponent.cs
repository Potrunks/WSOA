using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.GetAllAccounts.Component
{
    public class GetAllAccountsComponent : SubSectionComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public List<AccountViewModel> AccountViewModels { get; set; }

        public IDictionary<int, AccountViewModel> AccountViewModelDictionary { get; set; }

        public AccountViewModel AccountViewModelSelected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<List<AccountViewModel>> result = await AccountService.GetAllAccountViewModels();
            if (!result.Success)
            {
                string redirectUrl = !string.IsNullOrEmpty(result.RedirectUrl) ?
                                     result.RedirectUrl :
                                     string.Format("/main/error/{0}", "Une erreur est survenue pendant le chargement des comptes. Contactez un administrateur");
                NavigationManager.NavigateTo(redirectUrl);
                return;
            }

            AccountViewModels = result.Data.OrderBy(data => data.LastName).ThenBy(data => data.FirstName).ToList();

            AccountViewModelSelected = AccountViewModels.First();

            AccountViewModelDictionary = new Dictionary<int, AccountViewModel>();
            for (int i = 0; i < AccountViewModels.Count; i++)
            {
                AccountViewModelDictionary.Add(i, AccountViewModels[i]);
            }

            IsLoading = false;
        }

        public EventCallback<ChangeEventArgs> ChangeSelectedAccount => EventCallback.Factory.Create(this, (ChangeEventArgs args) =>
        {
            AccountViewModelSelected = AccountViewModelDictionary[int.Parse(args.Value!.ToString()!)];
        });
    }
}
