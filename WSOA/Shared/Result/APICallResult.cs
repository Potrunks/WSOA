namespace WSOA.Shared.Result
{
    public class APICallResult
    {
        public APICallResult()
        {
            Success = true;
            ErrorMessage = null;
        }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
