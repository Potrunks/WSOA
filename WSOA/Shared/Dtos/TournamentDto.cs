using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class TournamentDto
    {
        public TournamentDto()
        {
            Tournament = null;
            Players = new List<PlayerDto>();
            Address = null;
        }

        public TournamentDto(Tournament tournament, IEnumerable<PlayerDto> players, Address address)
        {
            Tournament = tournament;
            Players = players;
            Address = address;
        }

        public Tournament Tournament { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; }

        public Address Address { get; set; }
    }
}
