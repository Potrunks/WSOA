using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Resources;
using WSOA.Client.Utils;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APICallResult> CreateAccount(AccountCreationFormViewModel form)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync(ApiRouteResources.CREATE_ACCOUNT, form.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }

        public async Task<APICallResult> CreateLinkAccountCreation(LinkAccountCreationFormViewModel formViewModel)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync(ApiRouteResources.CREATE_LINK_ACCOUNT_CREATION, formViewModel.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }

        public async Task<InviteCallResult> LoadInviteDatas(int subSectionId)
        {
            HttpResponseMessage rep = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_INVITE_DATAS, subSectionId));
            return rep.Content.ToObject<InviteCallResult>();
        }

        public async Task<APICallResult> LogOut()
        {
            HttpResponseMessage rep = await _httpClient.GetAsync(ApiRouteResources.LOG_OUT);
            return rep.Content.ToObject<APICallResult>();
        }

        public async Task<APICallResult> SignIn(SignInFormViewModel signInFormVM)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync(ApiRouteResources.SIGN_IN, signInFormVM.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }
    }
}
