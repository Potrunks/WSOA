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

        public string WarningMessage
        {
            get
            {
                return string.IsNullOrWhiteSpace(_warningMessage) ? string.Empty :
                       _warningMessage.Contains("Un avertissement sauvage apparait") ? _warningMessage :
                       $"Un avertissement sauvage apparait : {_warningMessage}. Rien de grave mais préviens juste l'administrateur";
            }
            set
            {
                _warningMessage = value ?? string.Empty;
            }
        }
        private string _warningMessage;

        public string? RedirectUrl { get; set; }
    }
}
