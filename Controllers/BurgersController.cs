using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using burgershack.Models;
using burgershack.Repositories;
using System;

namespace burgershack.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class BurgersController : Controller
  {
    private BurgersRepository _repo;

    [HttpGet]
    public IEnumerable<Burger> Get()
    {
      return _repo.GetAll(); ;
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
      throw new Exception("INVALID BURGER");
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put([FromBody] Burger burger)
    {
      _repo.Update(burger);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _repo.Delete(id);

    }

    public BurgersController(BurgersRepository repo)
    {
      _repo = repo;
    }
  }
}