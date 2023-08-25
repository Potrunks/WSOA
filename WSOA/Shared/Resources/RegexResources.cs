namespace WSOA.Shared.Resources
{
    public static class RegexResources
    {
        public const string MAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string NAME = @"^[a-zA-ZÀ-ÿ\-']{1,50}$";
        public const string PASSWORD = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public const string LOGIN = @"^[a-zA-Z0-9_]{4,20}$";
    }
}
