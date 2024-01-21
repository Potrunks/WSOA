namespace WSOA.Shared.Resources
{
    public static class RouteResources
    {
        public const string MAIN_ERROR = "/main/error/{0}";
        public const string LOAD_TOURNAMENT_IN_PROGRESS = "api/tournament/inProgress/load/{0}";
        public const string ELIMINATE_PLAYER = "api/tournament/eliminatePlayer";
        public const string SAVE_BONUS_EARNED = "api/tournament/saveBonusEarned";
        public const string DELETE_BONUS_EARNED = "api/tournament/deleteBonusEarned";
        public const string CANCEL_PLAYER_ELIMINATION = "api/tournament/cancel/player/elimination";
        public const string EDIT_ADDON_PLAYER = "api/tournament/player/{0}/addon/{1}";
        public const string REMOVE_PLAYER_NVR_COME = "api/tournament/player/{0}/neverCome";
        public const string CANCEL_TOURNAMENT_IN_PROGRESS = "api/tournament/inProgress/{0}/cancel";
        public const string HOME = "/home";
    }
}
