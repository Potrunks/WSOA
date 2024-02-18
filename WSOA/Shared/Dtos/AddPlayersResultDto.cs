using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class AddPlayersResultDto
    {
        public AddPlayersResultDto() { }

        public IEnumerable<PlayerPlayingDto> AddedPlayersPlaying { get; set; }

        public IEnumerable<JackpotDistribution> UpdatedJackpotDistributions { get; set; }
    }
}
