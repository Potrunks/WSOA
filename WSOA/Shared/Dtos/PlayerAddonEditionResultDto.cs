using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class PlayerAddonEditionResultDto
    {
        public PlayerAddonEditionResultDto() { }

        public Player PlayerUpdated { get; set; }

        public IEnumerable<JackpotDistribution> JackpotDistributionsUpdated { get; set; }
    }
}
