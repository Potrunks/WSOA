namespace WSOA.Shared.Result
{
    public class APICallResultBase
    {
        public APICallResultBase()
        {

        }

        public APICallResultBase(bool success, string? redirectUrl = null)
        {
            Success = success;
            RedirectUrl = redirectUrl;
        }

        public APICallResultBase(string errorMsg, string? redirectUrl = null)
        {
            Success = false;
            ErrorMessage = errorMsg;
            RedirectUrl = redirectUrl;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? WarningMessage { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
