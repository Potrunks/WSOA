namespace WSOA.Server.Business.Resources
{
    public static class AccountBusinessResources
    {
        public const string ERROR_SIGN_IN = "Login ou mot de passe incorrect";
        public const string TECHNICAL_ERROR_SIGN_IN = "Erreur technique pendant la tentative d'authentification -> {0}";
        public const string TECHNICAL_ERROR_LINK_ACCOUNT_CREATION = "Erreur technique pendant la tentative de création d'un lien d'invitation -> {0}";
        public const string TECHNICAL_ERROR_INVITE_PAGE_LOADING = "Erreur technique pendant la tentative de chargement de la page d'invitation -> {0}";
        public const int LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY = 1;
        public const string LINK_ACCOUNT_CREATION_MAIL_SUBJECT = "World Series of Antoine : Creation de compte";
    }
}
