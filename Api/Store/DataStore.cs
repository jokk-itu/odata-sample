using Api.Models;

namespace Api.Store;

public class DataStore : IDataStore
{
  private static readonly string[] CustomerNames = ["John", "Jane"];
  private static readonly string[] ProductNames = ["Banana", "Pear", "Apple"];

  public IQueryable<Customer> Customers { get; }
  public IQueryable<Order> Orders { get; }
  public IQueryable<Product> Products { get; }

  public DataStore()
  {
    Customers = Enumerable
      .Range(1, 100)
      .Select(i => new Customer
      {
        Id = i,
        Name = CustomerNames[Random.Shared.Next(0, CustomerNames.Length)]
      })
      .AsQueryable();

    Products = Enumerable
      .Range(1, 100)
      .Select(i => new Product
      {
          Id = i,
          Name = ProductNames[Random.Shared.Next(0, ProductNames.Length)]
      })
      .AsQueryable();
  }
}
