using Newtonsoft.Json;
using POLYGLOT.Project.Pay.application.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace POLYGLOT.Project.Pay.infraestructure.Repositories
{
    public class RabbitMQRepository : IRabbitMQ
    {
        private readonly IConnectionFactory _factory;

        public RabbitMQRepository(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> PublishQueue(object data, string exchange, string routingKey)
        {
            try
            {
                var serializedMessage = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(serializedMessage);
                var connection = await _factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();
                await channel.BasicPublishAsync(exchange: exchange, routingKey: routingKey, body: body);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
