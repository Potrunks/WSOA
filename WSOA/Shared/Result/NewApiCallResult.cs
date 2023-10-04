namespace WSOA.Shared.Result
{
    public class NewApiCallResult<T>
    {
        public NewApiCallResult()
        {

        }

        public NewApiCallResult(bool success)
        {
            Success = success;
        }

        public NewApiCallResult(string errorMsg, string? redirectUrl = null)
        {
            Success = false;
            ErrorMessage = errorMsg;
            RedirectUrl = redirectUrl;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? WarningMessage { get; set; }

        public string? RedirectUrl { get; set; }

        public T Data { get; set; }
    }
}
