namespace WSOA.Client.Shared.Resources
{
    public static class ApiRouteResources
    {
        public const string CREATE_LINK_ACCOUNT_CREATION = "api/account/invite/createLink";
        public const string LOAD_INVITE_DATAS = "api/account/invite/{0}";
        public const string SIGN_IN = "api/account/signIn";
        public const string CREATE_ACCOUNT = "api/account/create";
        public const string LOG_OUT = "api/account/logOut";
        public const string CREATE_TOURNAMENT = "api/tournament/create";
        public const string LOAD_CREATE_TOURNAMENT_DATAS = "api/tournament/create/load/{0}";
        public const string LOAD_TOURNAMENTS_NOT_OVER = "api/tournament/future/load/{0}";
        public const string CLEAR_SESSION = "api/account/clearSession";
        public const string SIGN_UP_TOURNAMENT = "api/tournament/future/signUp";
        public const string TOURNAMENT_PREPARATION_GET_PLAYERS = "api/tournament/load/presentPlayers/{0}";
        public const string PLAY_TOURNAMENT_PREPARED = "api/tournament/prepared/save";
        public const string ADD_PLAYERS_TOURNAMENT_IN_PROGRESS = "api/tournament/inProgress/{0}/load/newPlayers";
        public const string SEND_MAIL_RESET_LOGIN = "api/account/reset/send/mail";
        public const string RESET_LOGIN = "api/account/reset/login";
        public const string GET_RESET_PWD_ACCOUNT_VM = "api/account/{0}/{1}/get";
        public const string GET_ALL_ACCOUNTS_VM = "api/account/get/all";
    }
}
