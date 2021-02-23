using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DiaryApp
{
  class DbController
  {
    //**************************************************************************
    //entry section
    //**************************************************************************
    //retrieve all entrys from database
    public List<DiaryEntryModel> GetEntrysFromDb(int userId)
    {
      using var db = new DiaryContext();
      return (from b in db.DiaryEntrys
              where b.UserId == userId
              select b).OrderByDescending(d => d.Date).ToList();

    }

    //Save the Created Entry to Database
    public void EntryToDb(DiaryEntryModel entry)
    {
      using var db = new DiaryContext();
      var result = db.DiaryEntrys.SingleOrDefault(e => e.EntryId == entry.EntryId);
      if (result != null)
      {
        db.Entry(result).CurrentValues.SetValues(entry);
      }
      else
      {
        db.DiaryEntrys.Add(entry);
      }
      db.SaveChanges();
    }

    //Delete selected entry from Database
    public void DeleteEntryInDb(DiaryEntryModel entrys)
    {
      using var db = new DiaryContext();
      db.Entry(entrys).State = EntityState.Deleted;
      db.SaveChanges();
    }
    //**************************************************************************
    //user section
    //**************************************************************************

    //Save the Created Entry to Database
    public void UserToDb(UserModel user)
    {
      using var db = new DiaryContext();
      db.Users.Add(user);
      db.SaveChanges();
    }

    public List<UserModel> GetUserName(string userName)
    {
      using var db = new DiaryContext();
      return (from b in db.Users
              where b.UserName == userName
              select b).ToList();
    }

    //Get the username from database
    public string GetFullName(int userId)
    {
      using var db = new DiaryContext();
      var query = (from b in db.Users where b.UserId == userId select b).SingleOrDefault();
      return query.FullName;
    }
  }
}






