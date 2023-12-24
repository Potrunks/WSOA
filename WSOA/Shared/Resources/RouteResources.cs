namespace WSOA.Shared.Resources
{
    public static class RouteResources
    {
        public const string MAIN_ERROR = "/main/error/{0}";
        public const string LOAD_TOURNAMENT_IN_PROGRESS = "api/tournament/inProgress/load/{0}";
        public const string ELIMINATE_PLAYER = "api/tournament/eliminatePlayer";
        public const string SAVE_BONUS_EARNED = "api/tournament/saveBonusEarned";
        public const string DELETE_BONUS_EARNED = "api/tournament/deleteBonusEarned";
        public const string CANCEL_PLAYER_ELIMINATION = "api/tournament/cancel/player/{0}/elimination";
        public const string EDIT_ADDON_PLAYER = "api/tournament/player/{0}/addon/{1}";
    }
}
