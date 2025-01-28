namespace POLYGLOT.Project.Security.application.Exceptions
{
    public class AuthError
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public bool Error { get; set; }
    }
}
