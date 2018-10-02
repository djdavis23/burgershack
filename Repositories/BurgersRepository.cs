using System.Collections.Generic;
using System.Data;
using burgershack.Models;
using Dapper;
using System.Linq;

namespace burgershack.Repositories
{
  public class BurgersRepository
  {
    //this class is responsible for talking to the database (i.e. DB service)

    private IDbConnection _db;


    public BurgersRepository(IDbConnection db)
    {
      _db = db;
    }

    //CRUD VIA SQL

    //GET ALL BURGERS
    public IEnumerable<Burger> GetAll()
    {
      //will run the query and either return a list of burgers or an empty list
      return _db.Query<Burger>($"SELECT * FROM Burgers");
    }

    //GET BURGER BY ID

    public Burger GetById(int id)
    {
      //@ - the parameter passed in
      //new {id} - dynamic object, used to pass input to the database, triggers dapper to
      //parse the object into a string and remove any sql commands - protects your object
      //this object is like { id: id}
      //FirstorDefault() returns the first element in the array or a null
      return _db.Query<Burger>("SELECT * FROM Burgers WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //CREATE BURGER
    public Burger Create(Burger burger)
    {
      //first @ indicates multi-line input
      //order of values must matach order of insert statement columns
      //DON"T USE STRING INTERPOLATION HERE, ESPECIALLY WITH USER INPUT
      //burger in line 52 is the object that dapper parses and references
      //from the values parameters two lines above
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO Burgers (name, description, price)
        VALUES (@Name, @Description, @Price);
        SELECT LAST_INSERT_ID();", burger
      );
      burger.Id = id;
      return burger;
    }

    //UPDATE BURGER
    public Burger Update(Burger burger)
    {
      _db.Execute(@"
      UPDATE Burgers SET (name, descriptions, price
      VALUES (@Name, @Description, @Price)
      WHERE id = @Id;", burger);
      return burger;
    }
    //DELETE BURGER
    public int Delete(int id)
    {
      return _db.Execute("DELETE FROM Burgers WHERE id=@Id;", new { id });

    }
  }
}