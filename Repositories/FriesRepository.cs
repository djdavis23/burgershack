
using System.Collections.Generic;
using System.Data;
using System.Linq;
using burgershack.Models;
using Dapper;

namespace burgershack.Repositories
{
  public class FriesRepository
  {
    private IDbConnection _db;

    public FriesRepository(IDbConnection db)
    {
      _db = db;
    }

    //GET ALL FRIES
    public IEnumerable<Fries> GetAll()
    {
      return _db.Query<Fries>("SELECT * FROM Fries;");
    }

    //GET FRIES BY ID
    public Fries GetById(int id)
    {
      return _db.Query<Fries>("SELECT * FROM Fries WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //CREATE FRIES
    public Fries Create(Fries fries)
    {
      int id = _db.ExecuteScalar<int>(@"
      INSERT INTO Fries (name, description, price)
      VALUES (@Name, @Description, @Price);
      SELECT LAST_INSERT_ID();", fries);
      fries.Id = id;
      return fries;
    }
    //UPDATE FRIES BY ID
    //Better practice for put and delete to return #rows modified as returned
    //from Execute?????
    public Fries Update(Fries fries)
    {
      _db.Execute(@"
        UPDATE Fries SET name = @Name, description = @Description, price = @Price
        WHERE id = @Id;", fries);
      return fries;
    }

    //DELETE FRIES BY ID   
    public int Delete(int id)
    {
      return _db.Execute("DELETE FROM Fries WHERE id = @id", new { id });
    }
  }
}
