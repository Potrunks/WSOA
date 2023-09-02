namespace WSOA.Shared.Exceptions
{
    public class FunctionalException : Exception
    {
        public string? RedirectUrl { get; set; }

        public FunctionalException() : base()
        {

        }

        public FunctionalException(string message, string? redirectUrl) : base(message)
        {
            RedirectUrl = redirectUrl;
        }
    }
}
