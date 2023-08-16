using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Invite.Components
{
    public class InviteComponent : ComponentBase
    {
        public LinkAccountCreationViewModel _viewModel = new LinkAccountCreationViewModel();

        public EditContext _editContext;

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_viewModel);
            _editContext.EnableDataAnnotationsValidation();
        }
    }
}
