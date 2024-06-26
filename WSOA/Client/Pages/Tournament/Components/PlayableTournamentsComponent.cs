﻿using Microsoft.AspNetCore.Components;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class PlayableTournamentsComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public TournamentsViewModel ViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            APICallResult<TournamentsViewModel> result = await TournamentService.LoadTournamentsNotOver(SubSectionId);
            if (!string.IsNullOrWhiteSpace(result.RedirectUrl))
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
                return;
            }
            ViewModel = result.Data;
            Description = result.Data.Description;

            IsLoading = false;
        }

        public EventCallback<int> DeleteTournamentSelectedIntoViewModel => EventCallback.Factory.Create(this, (int tournamentDeletedId) =>
        {
            ViewModel.TournamentsVM = ViewModel.TournamentsVM.Where(tou => tou.TournamentId != tournamentDeletedId).ToList();
        });
    }
}
