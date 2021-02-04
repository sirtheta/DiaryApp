using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DiaryApp
{
  public class DiaryContext : DbContext
  {
    public DbSet<DiaryEntryDb> DiaryEntrys { get; set; }
    public DbSet<UserDb> Users { get; set; }
  }

  class Model
  {

    public int GetUserId(string userName)
    {
      using (var db = new DiaryContext())
      {
        return (from b in db.Users
                          where b.UserName == userName
                          select b.UserId).SingleOrDefault();
      }
    }




    //retrieve all entrys from database
    public static List<DiaryEntryDb> GetEntrysFromDb(int userId)
    {
      using (var db = new DiaryContext())
      {
        return (from b in db.DiaryEntrys
                where b.UserId == userId
                orderby b.EntryId
                select b).ToList();
      }
    }

    //Get the username from database
    public string FullName()
    {
      using (var db = new DiaryContext())
      {
        var query = (from b in db.Users
                     where b.UserId == Control.UserId
                     select b).SingleOrDefault();
        return $"{query.FirstName} {query.LastName}";
      }
    }

    //Save the Created Entry to Database
    public void EntryToDb(DiaryEntryDb newEntry)
    {
      using (var db = new DiaryContext())
      {
        db.DiaryEntrys.Add(newEntry);
        db.SaveChanges();
      }
    }

    //Delete selected entry from Database
    public void DeleteEntryInDb(DiaryEntryDb entrys)
    {
      using (var db = new DiaryContext())
      {
        db.Entry(entrys).State = EntityState.Deleted;
        db.SaveChanges();
      }
    }
  }
  #region CreateTestEntrys

  public static class DatabaseInitializer
  {
    public static void CreateTestUser()
    {
      using (var db = new DiaryContext())
      {
        if (!db.Users.Any())
        {
          List<UserDb> lstUser = new List<UserDb>() { new UserDb { UserName = "1", FirstName = "User", LastName = "Example", Password = "1" } };
          lstUser.Add(new UserDb { UserName = "2", FirstName = "User2", LastName = "Example2", Password = "2" });
          db.Users.Add(lstUser[0]);
          db.Users.Add(lstUser[1]);
          db.SaveChanges();
        }
      }
    }


    public static void CreateTestEntrys()
    {
      string testText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
      using (var db = new DiaryContext())
      {
        //add some test entrys to the EntryDb for TestUser 1 and 2
        if (!db.DiaryEntrys.Any())
        {
          List<DiaryEntryDb> lstTestEntrys = new List<DiaryEntryDb>()
          { new DiaryEntryDb
            { Text = testText,
              TagBirthday = true,
              TagFriends = true, UserId = 1,
              Date = DateTime.Now.ToString("dd. MMMM yyyy")
            }
          };
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagBirthday = true,
            TagFamily = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-1).ToString("dd. MMMM yyyy")
          });
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagFamily = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-2).ToString("dd. MMMM yyyy")
          });
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagBirthday = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-3).ToString("dd. MMMM yyyy")
          });
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagFriends = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-4).ToString("dd. MMMM yyyy")
          });
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagFriends = true,
            TagBirthday = true,
            TagFamily = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-5).ToString("dd. MMMM yyyy")
          });
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = "this is user Id 2 and not visible with userId 1.",
            TagFriends = true,
            TagBirthday = true,
            TagFamily = true,
            UserId = 2,
            Date = DateTime.Now.AddDays(-6).ToString("dd. MMMM yyyy")
          });
          //more entrys
          for (int i = 6; i < 200; i++)
          {
            lstTestEntrys.Add(new DiaryEntryDb
            {
              Text = testText,
              TagFriends = true,
              TagBirthday = true,
              TagFamily = true,
              UserId = 1,
              Date = DateTime.Now.AddDays(-i).ToString("dd. MMMM yyyy")
            });
          }



          foreach (var item in lstTestEntrys)
          {
            db.DiaryEntrys.Add(item);
          }
          db.SaveChanges();
        }
      }
    }
  }
  #endregion
}






