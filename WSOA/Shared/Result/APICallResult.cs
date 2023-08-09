namespace WSOA.Shared.Result
{
    public class APICallResult
    {
        public APICallResult()
        {
            Success = true;
            ErrorMessage = null;
        }

        public APICallResult(bool success, string? errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
