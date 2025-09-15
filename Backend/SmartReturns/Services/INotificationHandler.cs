using SmartReturns.Consumers;

namespace SmartReturns.Services
{
    public interface INotificationHandler
    {
        void Handle(ReturnCreatedEvent evt);
    }
}
