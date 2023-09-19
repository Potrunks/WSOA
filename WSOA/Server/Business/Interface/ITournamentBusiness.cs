﻿using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Interface
{
    public interface ITournamentBusiness
    {
        /// <summary>
        /// Create new tournament and prevent all users in app.
        /// </summary>
        APICallResult CreateTournament(TournamentCreationFormViewModel form, ISession session);

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        CreateTournamentCallResult LoadTournamentCreationDatas(int subSectionId, ISession session);

        /// <summary>
        /// Load future tournament datas.
        /// </summary>
        LoadFutureTournamentCallResult LoadFutureTournamentDatas(int subSectionId, ISession session);

        /// <summary>
        /// Sign up the current user to the tournament selected.
        /// </summary>
        SignUpTournamentCallResult SignUpTournament(SignUpTournamentFormViewModel formVM, ISession session);
    }
}