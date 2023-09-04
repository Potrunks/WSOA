namespace WSOA.Shared.Result
{
    public class APICallResult
    {
        public APICallResult()
        {
            Success = false;
            ErrorMessage = null;
            RedirectUrl = null;
            WarningMessage = null;
        }

        public APICallResult(string? redirectUrl)
        {
            Success = true;
            ErrorMessage = null;
            RedirectUrl = redirectUrl;
            WarningMessage = null;
        }

        public APICallResult(string errorMsg, string? redirectUrl)
        {
            Success = false;
            ErrorMessage = errorMsg;
            RedirectUrl = redirectUrl;
            WarningMessage = null;
        }

        public APICallResult(bool isSuccess, string warningMsg, string? redirectUrl)
        {
            Success = isSuccess;
            ErrorMessage = null;
            RedirectUrl = redirectUrl;
            WarningMessage = warningMsg;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? RedirectUrl { get; set; }

        public string? WarningMessage { get; set; }

        public object Data { get; set; }
    }
}
