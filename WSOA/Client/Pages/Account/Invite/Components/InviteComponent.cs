using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Invite.Components
{
    public class InviteComponent : ActionMenuComponentBase
    {
        public LinkAccountCreationFormViewModel _formVM = new LinkAccountCreationFormViewModel();

        public InviteViewModel _inviteVM = new InviteViewModel();

        public EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

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

            IsLoading = false;
        }

        public Func<Task<APICallResult>> CreateLinkAccountCreation()
        {
            return () => AccountService.CreateLinkAccountCreation(_formVM);
        }
    }
}
