using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Order.Application.Interfaces;

namespace Order.Infrastructure.Services;

public class ServiceBusPublisher : IMessagePublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(IConfiguration config)
    {
        var client = new ServiceBusClient(config["ServiceBus:ConnectionString"]);
        _sender = client.CreateSender(config["ServiceBus:QueueName"]);
    }

    public async Task PublishAsync(string message)
    {
        await _sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}