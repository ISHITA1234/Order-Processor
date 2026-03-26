using Microsoft.Azure.Cosmos;
using Order.Domain.Entities;
using Newtonsoft.Json;

namespace Order.Infrastructure.Services;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient client)
    {
        _container = client.GetContainer("OrdersDb", "Orders");
    }

    public async Task SaveOrderAsync(Order.Domain.Entities.Order order)
    {
        if (string.IsNullOrEmpty(order.Id))
        throw new Exception("ID IS NULL ❌");

        // await _container.CreateItemAsync(order, new PartitionKey(order.OrderId));

        // var json = JsonConvert.SerializeObject(order);

        // var document = JsonConvert.DeserializeObject<dynamic>(json);

        // await _container.CreateItemAsync(
        //     document,
        //     new PartitionKey(order.OrderId)
        // );

        var cosmosOrder = new
        {
            id = order.Id,
            orderId = order.OrderId,
            product = order.Product,
            amount = order.Amount,
            status = order.Status
        };

        await _container.CreateItemAsync(
            cosmosOrder,
            new PartitionKey(order.OrderId)
        );
    }
}