namespace POLYGLOT.Project.Pay.application.Dto
{
    public class ResponseSuccess<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
