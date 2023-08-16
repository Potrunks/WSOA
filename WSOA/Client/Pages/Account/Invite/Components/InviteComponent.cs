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

        public string _errorMessage = null;

        public bool _isLoading = true;

        public bool _isProcessing = false;

        public bool _isSuccessProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            // TODO : Faire un component Loading qui permet de ne pas avoir acces a la page tant qu'elle est pas chargé
            _isLoading = true;

            _editContext = new EditContext(_formVM);
            _editContext.EnableDataAnnotationsValidation();

            InviteCallResult result = await AccountService.LoadInviteDatas(SubSectionId);
            if (!result.Success)
            {
                if (result.RedirectUrl != null)
                {
                    NavigationManager.NavigateTo(result.RedirectUrl);
                    return;
                }

                _errorMessage = result.ErrorMessage;
                _isLoading = false;
                return;
            }

            _inviteVM = result.InviteVM;
            _formVM.ProfileCodeSelected = result.InviteVM.ProfileLabelsByCode.First().Key;
            _formVM.SubSectionIdConcerned = SubSectionId;

            _isLoading = false;
        }

        public async Task CreateLinkAccountCreation()
        {
            _isProcessing = true;
            _isSuccessProcessing = false;
            _errorMessage = null;

            if (!_editContext.Validate())
            {
                _isProcessing = false;
                return;
            }

            APICallResult result = await AccountService.CreateLinkAccountCreation(_formVM);
            if (!result.Success)
            {
                if (result.RedirectUrl != null)
                {
                    NavigationManager.NavigateTo(result.RedirectUrl);
                    return;
                }

                _errorMessage = result.ErrorMessage;
                _isProcessing = false;
                return;
            }

            _isSuccessProcessing = true;

            _isProcessing = false;
        }
    }
}
