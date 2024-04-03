namespace WSOA.Shared.Result
{
    public class APICallResult<T> : APICallResultBase
    {
        public APICallResult()
        {

        }

        public APICallResult(bool success) : base(success)
        {

        }

        public APICallResult(string errorMsg, string? redirectUrl = null) : base(errorMsg, redirectUrl)
        {

        }

        public T Data { get; set; }
    }
}
