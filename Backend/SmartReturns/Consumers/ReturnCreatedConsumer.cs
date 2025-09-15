using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartReturns.Services;
using System.Text;
using System.Text.Json;

namespace SmartReturns.Consumers
{
    public class ReturnCreatedConsumer
    {
        private readonly INotificationHandler _handler;

        public ReturnCreatedConsumer(INotificationHandler handler)
        {
            _handler = handler;
        }

        public async Task StartListening()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "returns",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var returnRequest = JsonSerializer.Deserialize<ReturnCreatedEvent>(message);

                if (returnRequest != null)
                {
                    _handler.Handle(returnRequest);
                }
            };

            await channel.BasicConsumeAsync(queue: "returns",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" [*] Listening for ReturnCreated events...");
            Console.ReadLine(); // Keep service alive
        }
    }

    public class ReturnCreatedEvent
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
