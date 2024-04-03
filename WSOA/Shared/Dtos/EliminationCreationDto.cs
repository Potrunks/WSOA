namespace WSOA.Shared.Dtos
{
    public class EliminationCreationDto : EliminationEditionDto
    {
        public EliminationCreationDto() : base() { }

        public int EliminatorPlayerId { get; set; }

        public bool HasReBuy { get; set; }

        public IDictionary<int, int> WinnableMoneyByPosition { get; set; }
    }
}
