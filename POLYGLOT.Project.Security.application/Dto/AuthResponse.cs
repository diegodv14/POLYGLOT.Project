namespace POLYGLOT.Project.Security.application.Dto
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
