using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Order.Application.Interfaces;

namespace Order.Infrastructure.Services;

public class ServiceBusPublisher : IMessagePublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(IConfiguration config)
    {
        var connectionString = config["ServiceBusConnection"];
        var queueName = config["ServiceBusQueueName"];

        var client = new ServiceBusClient(connectionString);
        _sender = client.CreateSender(queueName);
    }

    public async Task PublishAsync(string message)
    {
        await _sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}