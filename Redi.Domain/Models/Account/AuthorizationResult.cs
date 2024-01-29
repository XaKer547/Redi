namespace Redi.Domain.Models.Account
{
    public class AuthorizationResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
    }
}
