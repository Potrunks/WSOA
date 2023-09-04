using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Components;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Pages.Tournament.Components
{
    public class CreateTournamentComponent : SubSectionComponentBase
    {
        [Inject]
        public ITournamentService TournamentService { get; set; }

        public TournamentCreationFormViewModel Form { get; set; }

        public EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            IsLoading = true;

            Form = new TournamentCreationFormViewModel();
            Form.BaseUri = NavigationManager.BaseUri;
            Form.SubSectionId = SubSectionId;

            EditContext = new EditContext(Form);
            EditContext.EnableDataAnnotationsValidation();

            IsLoading = false;
        }

        public Func<Task<APICallResult>> CreateTournament()
        {
            return () => TournamentService.CreateTournament(Form);
        }
    }
}
