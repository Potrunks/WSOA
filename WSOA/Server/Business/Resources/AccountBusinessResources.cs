namespace WSOA.Server.Business.Resources
{
    public static class AccountBusinessResources
    {
        public const string ERROR_SIGN_IN = "Login ou mot de passe incorrect";
        public const string TECHNICAL_ERROR_SIGN_IN = "Erreur technique pendant la tentative d'authentification -> {0}";
        public const string TECHNICAL_ERROR_LINK_ACCOUNT_CREATION = "Erreur technique pendant la tentative de création d'un lien d'invitation -> {0}";
        public const int LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY = 1;
    }
}
