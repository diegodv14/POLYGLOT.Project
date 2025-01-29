namespace POLYGLOT.Project.Pay.application.Interfaces
{
    public interface IRabbitMQ
    {
        Task<bool> PublishQueue(object data, string exchange, string routingKey); 
    }
}
