namespace Api.Models;

public class Product
{
  public int Id { get; init; }
  public required string Name { get; init; }
  public IEnumerable<Order> Orders { get; init; } = new List<Order>();
}