using Api.Models;
using Api.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers;
public class CustomerController : ODataController
{
  private readonly IDataStore _dataStore;

  public CustomerController(IDataStore dataStore)
  {
    _dataStore = dataStore;
  }

  [EnableQuery]
  [HttpGet]
  public ActionResult<IQueryable<Customer>> Get()
  {
    return Ok(_dataStore.Customers);
  }
}
