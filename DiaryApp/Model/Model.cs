﻿using System;
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
    //**************************************************************************
    //entry section
    //**************************************************************************
    //retrieve all entrys from database
    public List<DiaryEntryDb> GetEntrysFromDb(int userId)
    {
      using var db = new DiaryContext();
      return (from b in db.DiaryEntrys
              where b.UserId == userId
              select b).OrderByDescending(d => d.Date).ToList();

    }

    //Save the Created Entry to Database
    public void EntryToDb(DiaryEntryDb entry)
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
    public void DeleteEntryInDb(DiaryEntryDb entrys)
    {
      using var db = new DiaryContext();
      db.Entry(entrys).State = EntityState.Deleted;
      db.SaveChanges();
    }
    //**************************************************************************
    //user section
    //**************************************************************************

    //Save the Created Entry to Database
    public void UserToDb(UserDb user)
    {
      using var db = new DiaryContext();
      db.Users.Add(user);
      db.SaveChanges();
    }

    public List<UserDb> GetUser(string userName)
    {
      using var db = new DiaryContext();
      return (from b in db.Users
              where b.UserName == userName
              select b).ToList();
    }

    //Get the username from database
    public string FullName(int userId)
    {
      using var db = new DiaryContext();
      var query = (from b in db.Users where b.UserId == userId select b).SingleOrDefault();
      return $"{query.FirstName} {query.LastName}";
    }
  }

  //**************************************************************************
  //Create test entrys
  //**************************************************************************
  #region CreateTestEntrys

  static class DatabaseInitializer
  {
    public static void CreateTestUser()
    {
      using var db = new DiaryContext();
      if (!db.Users.Any())
      {
        List<UserDb> lstUser = new List<UserDb>() { new UserDb { UserName = "1", FirstName = "User", LastName = "Example", Password = SecurePasswordHasher.Hash("1") } };
        lstUser.Add(new UserDb { UserName = "2", FirstName = "User2", LastName = "Example2", Password = SecurePasswordHasher.Hash("2") });
        db.Users.Add(lstUser[0]);
        db.Users.Add(lstUser[1]);
        db.SaveChanges();
      }
    }


    public static void CreateTestEntrys()
    {
      string testText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

      using var db = new DiaryContext();
      //add some test entrys to the EntryDb for TestUser 1 and 2
      if (!db.DiaryEntrys.Any())
      {
        List<DiaryEntryDb> lstTestEntrys = new List<DiaryEntryDb>()
          { new DiaryEntryDb
            { Text = testText,
              TagBirthday = true,
              TagFriends = true, UserId = 1,
              Date = DateTime.Now
            }
          };
        lstTestEntrys.Add(new DiaryEntryDb
        {
          Text = "this is user Id 2 and not visible with userId 1.",
          TagFriends = true,
          TagBirthday = true,
          TagFamily = true,
          UserId = 2,
          Date = DateTime.Now.AddDays(-6)
        });

        //more entrys
        for (int i = 6; i < 10; i++)
        {
          lstTestEntrys.Add(new DiaryEntryDb
          {
            Text = testText,
            TagFriends = true,
            TagBirthday = true,
            TagFamily = true,
            UserId = 1,
            Date = DateTime.Now.AddDays(-i)
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
  #endregion
}






