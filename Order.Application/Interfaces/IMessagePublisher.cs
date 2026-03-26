namespace Order.Application.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync(string message);
}