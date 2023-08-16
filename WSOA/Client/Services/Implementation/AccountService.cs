using WSOA.Client.Services.Interface;
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

        public async Task<APICallResult> CreateLinkAccountCreation(LinkAccountCreationFormViewModel formViewModel)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync("api/account/invite/createLink", formViewModel.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }

        public async Task<InviteCallResult> LoadInviteDatas(int subSectionId)
        {
            HttpResponseMessage rep = await _httpClient.GetAsync(string.Format("api/account/invite/{0}", subSectionId));
            return rep.Content.ToObject<InviteCallResult>();
        }

        public async Task<APICallResult> SignIn(SignInFormViewModel signInFormVM)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync("api/account/signIn", signInFormVM.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }
    }
}
