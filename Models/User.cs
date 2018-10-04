using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace burgershack.Models
{
  //login class --HELPER MODEL
  //contains just the info needed for login
  public class UserLogin
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
  }

  //Registration class -- HELPER MODEL
  //contains just the info needed for registration
  public class UserRegistration
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }

  public class User
  {

    public string Id { get; set; }

    [Required]
    public string Username { get; set; }


    //internal access - only files within the package
    [Required]
    internal string Hash { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public bool ActiveAccount { get; set; } = true;

    //ClaimsPrincipal is essentially a token, an id pointing at user object
    public ClaimsPrincipal _principal { get; private set; }

    //generate token
    internal void SetClaims()
    {
      var claims = new List<Claim>{
        //what data do you want to store with token
        new Claim(ClaimTypes.Email, Email),
        new Claim(ClaimTypes.Name, Id)//analagous to req.session.uid = id
      };
      var userIdentity = new ClaimsIdentity(claims, "login");
      _principal = new ClaimsPrincipal(userIdentity);
    }
  }
}