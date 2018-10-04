using System;
using System.Data;
using System.Linq;
using BCrypt.Net;
using burgershack.Models;
using Dapper;

namespace burgershack.Repositories
{
  public class UserRepository
  {
    private IDbConnection _db;


    //LOGIN
    public User Login(UserLogin creds)
    {
      //get user record based on email
      User user = _db.Query<User>(@"
    SELECT * FROM users WHERE email = @Email;
    ", creds).FirstOrDefault();

      //safety check for user found
      if (user == null) return null;

      //check password
      //can only use verify if you use the default SALT value
      bool validPass = BCrypt.Net.BCrypt.Verify(creds.Password, user.Hash);
      if (!validPass) return null;
      user.Hash = null;
      return user;
    }

    //REGISTER
    public User Register(UserRegistration data)
    {
      //generate user id
      string id = Guid.NewGuid().ToString();

      //hash the password
      string hash = BCrypt.Net.BCrypt.HashPassword(data.Password);

      //store to database
      int success = _db.Execute(@"      
      INSERT INTO users (id, username, email, hash)
      VALUES (@id, @username, @email, @hash);",
       new
       {
         id,
         username = data.Username,
         email = data.Email,
         hash
       });

      //execute returns number of rows affected
      if (success != 1) return null;

      return new User()
      {
        Id = id,
        Username = data.Username,
        Email = data.Email,
        //dont return the hash to the controller
        Hash = null
      };
    }

    //AUTHENTICATE
    public User GetUserById(string id)
    {
      User user = _db.Query<User>(@"
      SELECT * FROM users WHERE id = @id;",
      new { id }).FirstOrDefault();
      return user;
    }

    //update

    //change password

    //delete

    public UserRepository(IDbConnection db)
    {
      _db = db;
    }
  }
}