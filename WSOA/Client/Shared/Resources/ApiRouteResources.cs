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
        public const string LOAD_FUTURE_TOURNAMENT_DATAS = "api/tournament/future/load/{0}";
        public const string CLEAR_SESSION = "api/account/clearSession";
    }
}
