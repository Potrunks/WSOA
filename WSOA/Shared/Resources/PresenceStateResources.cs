namespace WSOA.Shared.Resources
{
    public static class PresenceStateResources
    {
        public const string PRESENT_CODE = "PRESENT";
        public const string MAYBE_CODE = "MAYBE";
        public const string ABSENT_CODE = "ABSENT";

        public static IDictionary<string, string> PRESENCE_LABELS_BY_CODE = new Dictionary<string, string>
        {
            { PRESENT_CODE, PRESENT_LABEL },
            { MAYBE_CODE, MAYBE_LABEL },
            { ABSENT_CODE, ABSENT_LABEL }
        };
        public const string PRESENT_LABEL = "Présent";
        public const string MAYBE_LABEL = "Peut-être";
        public const string ABSENT_LABEL = "Absent";
    }
}
