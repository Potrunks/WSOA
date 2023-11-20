namespace WSOA.Shared.Dtos
{
    public class EliminationDto
    {
        public EliminationDto() { }

        public int EliminatedPlayerId { get; set; }

        public int EliminatorPlayerId { get; set; }

        public bool HasReBuy { get; set; }

        public IDictionary<int, int> WinnableMoneyByPosition { get; set; }
    }
}
