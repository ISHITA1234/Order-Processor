using Microsoft.AspNetCore.Mvc;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Infrastructure.Services;
using Order.Application.Interfaces;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly CosmosDbService _db;
    private readonly IMessagePublisher _publisher;

    public OrderController(CosmosDbService db, IMessagePublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderDto dto)
    {
        var order = new Order.Domain.Entities.Order
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = Guid.NewGuid().ToString(),
            Product = dto.Product,
            Amount = dto.Amount,
            Status = "Pending"
        };

        Console.WriteLine($"ID VALUE: {order.Id}");

        await _db.SaveOrderAsync(order);
        await _publisher.PublishAsync(order.OrderId);

        return Ok(order.OrderId);
    }
}