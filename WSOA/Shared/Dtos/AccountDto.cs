namespace WSOA.Shared.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long? ForgotPasswordKey { get; set; }

        public DateTime? ForgotPasswordExpirationDate { get; set; }
    }
}
