using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

/// <summary>
/// Class to Initialize the database that one can see something without sign up first
/// </summary>
namespace DiaryApp
{
  //**************************************************************************
  //Create test entrys
  //**************************************************************************
  static class DatabaseInitializer
  {
    public static void CreateTestUser()
    {
      using var db = new DiaryContext();
      if (!db.Users.Any())//only create user if no users exist
      {
        List<UserModel> lstUser = new List<UserModel>() { new UserModel { UserName = "1", FirstName = "User", LastName = "Example", Password = SecurePasswordHasher.Hash("1") } };
        lstUser.Add(new UserModel { UserName = "2", FirstName = "User2", LastName = "Example2", Password = SecurePasswordHasher.Hash("2") });
        db.Users.Add(lstUser[0]);
        db.Users.Add(lstUser[1]);
        db.SaveChanges();
      }
    }

    public static void CreateTestEntrys()
    {
      using var db = new DiaryContext();
      //add some test entrys to the EntryDb for TestUser 1 and 2 if db is empty
      if (!db.DiaryEntrys.Any()) //only create entries if no entries exist
      {
        byte[] img;
        string testText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor " +
          "invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo " +
          "dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. " +
          "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et " +
          "dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet " +
          "clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
        using (WebClient webClient = new WebClient())
        {
          img = webClient.DownloadData("https://onaliternote.files.wordpress.com/2016/11/wp-1480230666843.jpg");
        }

        List<DiaryEntryModel> lstTestEntrys = new List<DiaryEntryModel>()
        { new DiaryEntryModel
          {
            UserId = 1,
            Text = testText,
            TagBirthday = true,
            TagFriends = true,
            ByteImage = img,
            Date = DateTime.Today
          },
          //create a sencond entry for user 2
          new DiaryEntryModel
          {
            UserId = 2,
            Text = "this is user Id 2 and not visible with userId 1.",
            TagFriends = true,
            TagBirthday = true,
            TagFamily = true,
            ByteImage = img,
            Date = DateTime.Today.AddDays(-6)
          }
        };

        //more entrys
        for (int i = 6; i < 50; i++)
        {
          lstTestEntrys.Add(new DiaryEntryModel
          {
            UserId = 1,
            Text = testText,
            TagFriends = true,
            TagBirthday = true,
            TagFamily = true,
            ByteImage = img,
            Date = DateTime.Today.AddDays(-i)
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
