using Api.Models;
using Api.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers;
public class ProductController : ODataController
{
  private readonly IDataStore _dataStore;

  public ProductController(IDataStore dataStore)
  {
    _dataStore = dataStore;
  }

  [EnableQuery]
  [HttpGet]
  public ActionResult<IQueryable<Product>> Get()
  {
    return Ok(_dataStore.Products);
  }
}
