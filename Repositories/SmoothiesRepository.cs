using System.Collections.Generic;
using System.Data;
using burgershack.Models;
using Dapper;
using System.Linq;

namespace burgershack.Repositories
{
  public class SmoothiesRepository
  {
    //this class is responsible for talking to the database (i.e. DB service)

    private IDbConnection _db;


    public SmoothiesRepository(IDbConnection db)
    {
      _db = db;
    }

    //CRUD VIA SQL

    //GET ALL BURGERS
    public IEnumerable<Smoothie> GetAll()
    {
      //will run the query and either return a list of Smoothies or an empty list
      return _db.Query<Smoothie>($"SELECT * FROM Smoothies");
    }

    //GET Smoothie BY ID

    public Smoothie GetById(int id)
    {
      //@ - the parameter passed in
      //new {id} - dynamic object, used to pass input to the database, triggers dapper to
      //parse the object into a string and remove any sql commands - protects your object
      //this object is like { id: id}
      //FirstorDefault() returns the first element in the array or a null
      return _db.Query<Smoothie>("SELECT * FROM Smoothies WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //CREATE Smoothie
    public Smoothie Create(Smoothie smoothie)
    {
      //first @ indicates multi-line input
      //order of values must matach order of insert statement columns
      //DON"T USE STRING INTERPOLATION HERE, ESPECIALLY WITH USER INPUT
      //Smoothie in line 52 is the object that dapper parses and references
      //from the values parameters two lines above
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO Smoothies (name, description, price)
        VALUES (@Name, @Description, @Price);
        SELECT LAST_INSERT_ID();", smoothie
      );
      smoothie.Id = id;
      return smoothie;
    }

    //UPDATE Smoothie
    public Smoothie Update(Smoothie smoothie)
    {
      _db.Execute(@"
      UPDATE Smoothies SET name = @Name, description = @Description, price = @Price
      WHERE id = @Id;", smoothie);
      return smoothie;
    }
    //DELETE Smoothie
    public int Delete(int id)
    {
      return _db.Execute("DELETE FROM Smoothies WHERE id=@Id;", new { id });

    }
  }
}