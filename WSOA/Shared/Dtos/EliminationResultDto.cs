namespace WSOA.Shared.Dtos
{
    public class EliminationResultDto
    {
        public int Id { get; set; }

        public string FirstNameEliminator { get; set; }

        public string LastNameEliminator { get; set; }

        public int UserEliminatorId { get; set; }

        public int PlayerEliminatorId { get; set; }

        public string FirstNameVictim { get; set; }

        public string LastNameVictim { get; set; }

        public int UserVictimId { get; set; }

        public int PlayerVictimId { get; set; }
    }
}
