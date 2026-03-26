using Order.Application.Interfaces;
namespace Order.Application.Services;

public class OrderService
{
    private readonly IMessagePublisher _publisher;

    public OrderService(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task CreateOrder(string orderId)
    {
        await _publisher.PublishAsync(orderId);
    }
}