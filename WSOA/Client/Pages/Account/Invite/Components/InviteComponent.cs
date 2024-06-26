﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Account.Invite.Components
{
    public class InviteComponent : SubSectionComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public LinkAccountCreationFormViewModel _formVM = new LinkAccountCreationFormViewModel();

        public InviteViewModel _inviteVM = new InviteViewModel();

        public EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            _editContext = new EditContext(_formVM);
            _editContext.EnableDataAnnotationsValidation();

            APICallResult<InviteViewModel> result = await AccountService.LoadInviteDatas(SubSectionId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }

            _inviteVM = result.Data;
            _formVM.ProfileCodeSelected = result.Data.ProfileLabelsByCode.First().Key;
            _formVM.SubSectionIdConcerned = SubSectionId;
            _formVM.BaseUri = NavigationManager.BaseUri;

            IsLoading = false;
        }

        public Func<Task<APICallResultBase>> CreateLinkAccountCreation()
        {
            return () => AccountService.CreateLinkAccountCreation(_formVM);
        }
    }
}
