using RabbitMQ.Client;
using SmartReturns.Models;
using System.Text;
using System.Text.Json;

namespace SmartReturns.Services
{
    public class ReturnService : IReturnService
    {
        private readonly ReturnsDbContext _db;
        private readonly IConnection _rabbitConnection;

        public ReturnService(ReturnsDbContext db)
        {
            _db = db;
        }

        public async Task<ReturnRequest?> GetReturnAsync(int id)
        {
            return await _db.Returns.FindAsync(id);
        }

        public async Task<ReturnRequest> CreateReturnAsync(ReturnRequest request)
        {
            _db.Returns.Add(request);
            await _db.SaveChangesAsync();

            // Publish to RabbitMQ
            PublishReturnCreatedEvent(request);

            return request;
        }

        private async Task PublishReturnCreatedEvent(ReturnRequest request)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "returns",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "",
                                 routingKey: "returns",
                                 mandatory: false,
                                 body: body);

            Console.WriteLine($"[x] Published ReturnCreated event for ReturnId={request.Id}");
        }
    }
}
