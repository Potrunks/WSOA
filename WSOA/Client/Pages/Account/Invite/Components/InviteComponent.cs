using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Invite.Components
{
    public class InviteComponent : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int SubSectionId { get; set; }

        public LinkAccountCreationFormViewModel _formVM = new LinkAccountCreationFormViewModel();

        public InviteViewModel _inviteVM = new InviteViewModel();

        public EditContext _editContext;

        public bool _isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            _editContext = new EditContext(_formVM);
            _editContext.EnableDataAnnotationsValidation();

            InviteCallResult result = await AccountService.LoadInviteDatas(SubSectionId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            _inviteVM = result.InviteVM;
            _formVM.ProfileCodeSelected = result.InviteVM.ProfileLabelsByCode.First().Key;
            _formVM.SubSectionIdConcerned = SubSectionId;
            _formVM.BaseUri = NavigationManager.BaseUri;

            _isLoading = false;
        }

        public Func<Task<APICallResult>> CreateLinkAccountCreation()
        {
            return () => AccountService.CreateLinkAccountCreation(_formVM);
        }
    }
}
