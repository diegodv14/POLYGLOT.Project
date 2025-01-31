namespace POLYGLOT.Project.Transaction.application.Exceptions
{
    public class ResponseError
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public bool Error { get; set; }
    }
}
