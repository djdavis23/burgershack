using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using burgershack.Models;
using System;
using burgershack.Repositories;

namespace burgershack.Controllers
{
  //specify route, [controller] == Fries in this case, controller name
  [Route("api/[controller]")]
  [ApiController]
  public class FriesController : ControllerBase
  {
    private FriesRepository _repo;


    //Get all fries
    [HttpGet]
    public IEnumerable<Fries> Get()
    {
      return _repo.GetAll();
    }

    //get fries by id
    [HttpGet("{id}")]
    public Fries GetFriesById([FromRoute] int id)
    {
      return _repo.GetById(id);
    }

    //for post, can bring in a route parameter and a single data object
    [HttpPost]
    public Fries Post([FromBody] Fries fries)
    {
      //checks all data attributes from Fires are met
      if (ModelState.IsValid)
      {
        fries = new Fries(fries.Name, fries.Description, fries.Price);
        return _repo.Create(fries);
      }
      throw new Exception("INVALID FRIES OBJECT");
    }

    // PUT api/values/5
    [HttpPut]
    public void Put([FromBody] Fries fries)
    {
      _repo.Update(fries);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete([FromRoute] int id)
    {
      _repo.Delete(id);

    }

    public FriesController(FriesRepository repo)
    {
      _repo = repo;
    }
  }
}