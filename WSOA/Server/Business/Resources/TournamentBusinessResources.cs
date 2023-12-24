namespace WSOA.Server.Business.Resources
{
    public static class TournamentBusinessResources
    {
        public const string MAIL_SUBJECT_NEW_TOURNAMENT = "World Series Of Antoine : Nouveau tournoi";
        public const string MAIL_BODY_NEW_TOURNAMENT = "L'organisateur a créé un nouveau tournoi : {0}";
        public const string TOURNAMENT_ALREADY_OVER = "Le tournoi est déjà terminé. Impossible de s'inscrire";
        public const string CANNOT_EXECUTE_TOURNAMENT = "Le tournoi ne peut pas être executer car déjà en cours ou fini";
        public const string EXISTS_TOURNAMENT_IN_PROGRESS = "Il existe déjà un tournoi en cours. Impossible de lancer un autre tournoi";
        public const string TOTAL_ADDON_GIVEN_LESS_THAN_ZERO = "Le nombre d'add-on ne peut pas être inférieur à zéro";
        public const string PLAYER_NOT_IN_ADDON = "Le joueur n'est pas en add-on";
    }
}
