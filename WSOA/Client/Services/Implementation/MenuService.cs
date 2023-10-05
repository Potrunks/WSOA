using WSOA.Client.Services.Interface;
using WSOA.Client.Utils;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APICallResult<MainNavMenuViewModel>> LoadMainMenu()
        {
            HttpResponseMessage rep = await _httpClient.GetAsync("api/menu/loadMainNavMenu");
            return rep.Content.ToObject<APICallResult<MainNavMenuViewModel>>();
        }
    }
}
