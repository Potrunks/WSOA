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

        public async Task<APICallResult> SignIn(SignInFormViewModel signInFormVM)
        {
            HttpResponseMessage rep = await _httpClient.PostAsync("api/account/signIn", signInFormVM.ToJsonUtf8());
            return rep.Content.ToObject<APICallResult>();
        }
    }
}
