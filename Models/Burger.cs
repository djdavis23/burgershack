using System;
using System.ComponentModel.DataAnnotations;

namespace burgershack.Models
{
  public class Burger
  {
    public int Id { get; set; }

    [Required]
    [MinLength(6)]
    public string Name { get; set; }
    [MaxLength(255)]
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }

    //CONSTRUCTOR
    public Burger(string name, string description, decimal price)
    {

      Name = name;
      Description = description;
      Price = price;
    }
    //needed by MySql to build a burger from a returned record
    //maps columns into propery names
    public Burger()
    {

    }
  }
}