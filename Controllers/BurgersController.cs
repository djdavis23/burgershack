using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using burgershack.Models;
using burgershack.Repositories;
using System;

namespace burgershack.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class BurgersController : ControllerBase
  {
    private BurgersRepository _repo;

    [HttpGet]
    public IEnumerable<Burger> Get()
    {
      return _repo.GetAll(); ;
    }

    //get burgerk by id
    [HttpGet("{id}")]
    public Burger GetBurgerById([FromRoute] int id)
    {
      return _repo.GetById(id);
    }

    //for post, can bring in a route parameter and a single data object
    [HttpPost]
    public Burger Post([FromBody] Burger burger)
    {
      //checks all data attributes from Burger are met
      if (ModelState.IsValid)
      {
        burger = new Burger(burger.Name, burger.Description, burger.Price);
        return _repo.Create(burger);
      }
      throw new Exception("INVALID BURGER OBJECT");
    }

    // PUT api/values/5
    [HttpPut]
    public void Put([FromBody] Burger burger)
    {
      _repo.Update(burger);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete([FromRoute] int id)
    {
      _repo.Delete(id);

    }

    public BurgersController(BurgersRepository repo)
    {
      _repo = repo;
    }
  }
}