namespace WSOA.Shared.Result
{
    public class APICallResult
    {
        public APICallResult()
        {
            Success = false;
            ErrorMessage = null;
            RedirectUrl = null;
        }

        public APICallResult(string redirectUrl)
        {
            Success = true;
            ErrorMessage = null;
            RedirectUrl = redirectUrl;
        }

        public APICallResult(string errorMsg, string redirectUrl)
        {
            Success = false;
            ErrorMessage = errorMsg;
            RedirectUrl = redirectUrl;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
