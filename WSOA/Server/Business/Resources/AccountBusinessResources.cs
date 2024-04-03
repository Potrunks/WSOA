namespace WSOA.Server.Business.Resources
{
    public static class AccountBusinessResources
    {
        public const string ERROR_SIGN_IN = "Login ou mot de passe incorrect";
        public const string TECHNICAL_ERROR_SIGN_IN = "Erreur technique pendant la tentative d'authentification -> {0}";
        public const string TECHNICAL_ERROR_LINK_ACCOUNT_CREATION = "Erreur technique pendant la tentative de création d'un lien d'invitation -> {0}";
        public const string TECHNICAL_ERROR_INVITE_PAGE_LOADING = "Erreur technique pendant la tentative de chargement de la page d'invitation -> {0}";
        public const string TECHNICAL_ERROR_ACCOUNT_CREATION = "Erreur technique pendant la tentative de création de compte -> {0}";
        public const int LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY = 1;
        public const string LINK_ACCOUNT_CREATION_MAIL_SUBJECT = "World Series of Antoine : Creation de compte";
        public const string LINK_ACCOUNT_CREATION_EXTENDED = "Lien déjà généré. Extension de la date d'éxpiration";
        public const string LINK_ACCOUNT_CREATION_RE_SEND = "Lien déjà généré. Renvoie du lien par mail";
        public const string LINK_ACCOUNT_CREATION_NOT_EXIST_OR_EXPIRED = "Création de compte non autorisée ou expirée.";
        public const string LOGIN_ALREADY_EXISTS = "Ce login est déjà utilisé";
        public const string LOG_OUT = "Vous êtes deconnecté";
    }
}
