using SmartReturns.Consumers;

namespace SmartReturns.Services
{
    public class NotificationHandler : INotificationHandler
    {
        public void Handle(ReturnCreatedEvent evt)
        {
            // Mock notification logic
            Console.WriteLine($"[Notification] New return created: Id={evt.Id}, Product={evt.ProductId}, Reason={evt.Reason}");

            // TODO: Save to MongoDB or send an actual email
        }
    }
}
