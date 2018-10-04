using System.Threading.Tasks;
using burgershack.Models;
using burgershack.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//when we added and configured authentication in the Startup.cs file, we specified
//a login path of Account/Login.  Therefore, this controller MUST be called
//AccountController.

namespace burgershack.Controllers
{
  [Route("[controller]")]
  public class AccountController : Controller
  {
    private readonly UserRepository _repo;


    //Post is the standard request for login.  Need post vice get because you
    //have to pass user credential via a req body.  Get requests do not have
    //req body
    [HttpPost("Login")]
    //LOGIN
    //async because if valid, server needs to give client a cookie which takes
    //time and must complete before returning user data
    //Task - asynchronous, sets up multiple threads to handle process    
    public async Task<User> Login([FromBody] UserLogin creds)
    {
      //invalid input from user, return null
      if (!ModelState.IsValid) return null;

      //try login with DB
      User user = _repo.Login(creds);

      //if no record found, return null
      if (user == null) return null;

      //if user returned, set up web token and give back to client
      //usually a framework does this, not ME!!!!
      //i.e. GenerateUserClaim(user);
      //this sets up a claim (i.e. session) on the server
      user.SetClaims();
      //HttpContext equivalent to req
      //creates a session(claim), signs user in nand gives the token back
      //token is just an id that points to a claim which contains
      //the selected user indentification
      await HttpContext.SignInAsync(user._principal);
      return user;
    }

    //REGISTER
    [HttpPost("Register")]

    public async Task<User> Register([FromBody] UserRegistration data)
    {
      //check for valid input from client
      if (!ModelState.IsValid) return null;

      //attempt to create user record in DB
      User user = _repo.Register(data);

      //if create failed, return null
      if (user == null) return null;

      //configures user ClaimsPrincipal (WebToken)??
      user.SetClaims();
      //intializes a session (claim) and returns token to user
      await HttpContext.SignInAsync(user._principal);
      return user;
    }

    [HttpDelete("Logout")]
    public async Task<bool> Logout()
    {
      //ends session
      await HttpContext.SignOutAsync();
      return true;
    }
    //If the user has a valid token, this will return user data
    //So, this will persist a login through page refresh
    [Authorize]
    [HttpGet("Authenticate")]
    public User Authenticate()
    {
      var id = HttpContext.User.Identity.Name;
      return _repo.GetUserById(id);
    }

    //constructor - initializes repository
    public AccountController(UserRepository repo)
    {
      _repo = repo;
    }
  }
}