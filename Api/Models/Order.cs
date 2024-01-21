namespace Api.Models;

public class Order
{
  public int Id { get; init; }
  public int Amount { get; init; }
  public required Customer Customer { get; init; }
  public IEnumerable<Product> Products { get; init; } = new List<Product>();
}