namespace WSOA.Shared.Resources
{
    public static class TournamentMessageResources
    {
        public const string TOURNAMENT_NO_PLAYER_SELECTED = "Aucun joueur selectionné";
        public const string TOURNAMENT_IN_PROGRESS_ALREADY_STORED = "Des données sur un tournoi en cours sont déjà présentes. Veuillez contacter un administrateur";
        public const string NO_TOURNAMENT_IN_PROGRESS = "Aucun unique tournoi en cours";
        public const string NO_PLAYERS_PRESENT = "Aucun joueur inscrit présent pour le tournoi";
        public const string PLAYER_ALREADY_ELIMINATED = "Joueur déjà éliminé";
        public const string WHO_ELIMINATE_PLAYER = "Qui a éliminé {0} ?";
        public const string PLAYERS_ELIMINATION_CONCERNED_MISSING = "Le joueur eliminé et/ou eliminateur n'est pas dans la base de données. Player ID eliminé : {0}, Player ID eliminateur : {1}";
        public const string PLAYERS_ALREADY_DEFINITIVELY_ELIMINATED = "Le joueur eliminé et/ou eliminateur est déjà définitivement éliminé";
        public const string TOURNAMENT_PAST = "La date du tournoi est passé";
    }
}
