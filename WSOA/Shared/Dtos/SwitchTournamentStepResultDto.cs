using WSOA.Shared.Entity;
using WSOA.Shared.Resources;

namespace WSOA.Shared.Dtos
{
    public class SwitchTournamentStepResultDto
    {
        public SwitchTournamentStepResultDto() { }

        public TournamentStepEnum NewTournamentStep { get; set; }

        public IEnumerable<JackpotDistribution>? UpdatedJackpotDistributions { get; set; }
    }
}
