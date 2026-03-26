namespace Order.Domain.Entities;

public class Order
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string Product { get; set; }
    public double Amount { get; set; }
    public string Status { get; set; }
}