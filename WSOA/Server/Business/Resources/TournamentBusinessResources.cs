namespace WSOA.Server.Business.Resources
{
    public static class TournamentBusinessResources
    {
        public const string MAIL_SUBJECT_NEW_TOURNAMENT = "World Series Of Antoine : Nouveau tournoi";
        public const string MAIL_BODY_NEW_TOURNAMENT = "L'organisateur a créé un nouveau tournoi : {0}";
        public const string TOURNAMENT_ALREADY_OVER = "Le tournoi est déjà terminé. Impossible de s'inscrire";
        public const string CANNOT_EXECUTE_TOURNAMENT = "Le tournoi ne peut pas être executer car déjà en cours ou fini";
        public const string EXISTS_TOURNAMENT_IN_PROGRESS = "Il existe déjà un tournoi en cours. Impossible de lancer un autre tournoi";
    }
}
