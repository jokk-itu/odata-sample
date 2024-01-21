using Api.Models;

namespace Api.Store;

public interface IDataStore
{
  IQueryable<Customer> Customers { get; }
  IQueryable<Order> Orders { get; }
  IQueryable<Product> Products { get; }

}