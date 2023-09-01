namespace WSOA.Shared.Resources
{
    public static class DataValidationResources
    {
        public const string MAIL_MISSING = "Mail manquant";
        public const string MAIL_FORMAT_NO_VALID = "Format mail invalide. Exemple : mail@mail.com";
        public const string PROFILE_MISSING = "Profile manquant";
        public const string FIRSTNAME_MISSING = "Prénom manquant";
        public const string FIRSTNAME_FORMAT_NO_VALID = "Format prénom invalide. Lettres et caractères spéciaux autorisés. 50 caractères max";
        public const string LASTNAME_MISSING = "Nom manquant";
        public const string LASTNAME_FORMAT_NO_VALID = "Format nom invalide. Lettres et caractères spéciaux autorisés. 50 caractères max";
        public const string LOGIN_MISSING = "Login manquant";
        public const string LOGIN_FORMAT_NO_VALID = "Format login invalide. Lettres, chiffres et underscore autorisés. 4 à 20 caractères";
        public const string PASSWORD_MISSING = "Mot de passe manquant";
        public const string PASSWORD_FORMAT_NO_VALID = "Format mot de passe invalide. Une lettre majuscule, un caractère spécial, un chiffre et minimum 8 caractères";
        public const string PASSWORD_CONFIRMATION_MISSING = "Mot de passe de confirmation manquant";
        public const string PASSWORD_CONFIRMATION_NO_VALID = "Mot de passe de confirmation ne correspond pas au mot de passe saisi";
        public const string STARTDATE_MISSING = "Date de début manquante";
        public const string STARTDATE_ERROR_RANGE = "Date de début dans le passé interdit";
        public const string SEASON_MISSING = "Saison manquante";
        public const string BUY_IN_MISSING = "Buy-In manquant";
        public const string BUY_IN_ERROR_RANGE = "Buy-In doit être supérieur à 0";
        public const string ADDRESS_MISSING = "Adresse manquante";
    }
}
