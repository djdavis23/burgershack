using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using burgershack.Models;
using burgershack.Repositories;
using System;

namespace burgershack.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class SmoothiesController : ControllerBase
  {
    SmoothiesRepository _repo;

    [HttpGet]
    public IEnumerable<Smoothie> Get()
    {
      return _repo.GetAll();
    }

    [HttpGet("{id}")]
    public Smoothie GetSmoothieById([FromRoute] int id)
    {
      return _repo.GetById(id);
    }

    //for post, can bring in a route parameter and a single data object
    [HttpPost]
    public Smoothie Post([FromBody] Smoothie smoothie)
    {//checks all data attributes from Burger are met
      if (ModelState.IsValid)
      {
        smoothie = new Smoothie(smoothie.Name, smoothie.Description, smoothie.Price);
        return _repo.Create(smoothie);
      }
      throw new Exception("INVALID SMOOTHIE");

    }

    // PUT api/values/5
    [HttpPut]
    public void Put([FromBody] Smoothie smoothie)
    {
      _repo.Update(smoothie);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete([FromRoute] int id)
    {
      _repo.Delete(id);

    }

    public SmoothiesController(SmoothiesRepository repo)
    {
      _repo = repo;

    }
  }
}