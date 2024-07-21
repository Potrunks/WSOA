namespace WSOA.Shared.Resources
{
    public static class TournamentMessageResources
    {
        public const string TOURNAMENT_NO_PLAYER_SELECTED = "Aucun joueur selectionné";
        public const string TOURNAMENT_IN_PROGRESS_ALREADY_STORED = "Des données sur un tournoi en cours sont déjà présentes. Veuillez contacter un administrateur";
        public const string NO_TOURNAMENT_IN_PROGRESS = "Aucun unique tournoi en cours";
        public const string TOURNAMENT_FINISHED = "Le tournoi est terminé";
        public const string NO_PLAYERS_PRESENT = "Aucun joueur inscrit présent pour le tournoi";
        public const string PLAYER_ALREADY_ELIMINATED = "Joueur déjà éliminé";
        public const string WHO_ELIMINATE_PLAYER = "Qui a éliminé {0}?";
        public const string WHICH_BONUS_HAS_WON = "Quel bonus a gagné {0}?";
        public const string WHICH_BONUS_TO_DELETE = "Quel bonus de {0} est retiré?";
        public const string WANT_REBUY = "Re-Buy?";
        public const string PLAYERS_ELIMINATION_CONCERNED_MISSING = "Le joueur eliminé et/ou eliminateur n'est pas dans la base de données. Player ID eliminé : {0}, Player ID eliminateur : {1}";
        public const string PLAYERS_ALREADY_DEFINITIVELY_ELIMINATED = "Le joueur eliminé et/ou eliminateur est déjà définitivement éliminé";
        public const string TOURNAMENT_PAST = "La date du tournoi est passé";
        public const string HOW_MUCH_ADDON_FOR_PLAYER = "Combien d'add-on pour {0}";
        public const string PLAYER_ALREADY_NOT_PRESENT = "Le joueur était déjà non présent. L'action a été réalisé malgré tout.";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_REBUY_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà rebuy";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_ADDON_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà fait l'addon";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_FINAL_TABLE_ERROR = "Le joueur ne peut pas être retiré du tournoi car il est déjà en table finale";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_PTS_EARNED_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà gagné des points";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_POSITION_EARNED_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà une position dans le tournoi";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_IN_PROGRESS_ERROR = "Le joueur ne peut pas être retiré du tournoi car le tournoi n'est pas en cours";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_ELIMINATOR_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà éliminé au moins un joueur";
        public const string REMOVE_PLAYER_FROM_TOURNAMENT_BONUS_ERROR = "Le joueur ne peut pas être retiré du tournoi car il a déjà gagné un bonus";
        public const string TOURNAMENT_IN_PROGRESS_CANCELLED = "Le tournoi en cours a été annulé";
    }
}
