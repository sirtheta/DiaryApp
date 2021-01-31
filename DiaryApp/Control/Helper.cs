using DiaryApp.Model;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DiaryApp.Control
{
  class Helper
  {
    public static bool ShowMessageBox(string messageStr, MessageType type, MessageButtons buttons)
    {
      return (bool)new CustomMessageBox(messageStr, type, buttons).ShowDialog();
    }

    public static void ShowNotification(string titel, string message, NotificationType type)
    {
      var notificationManager = new NotificationManager();
      notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
              areaName: "WindowArea");
    }
  }

  static class Globals
  {
    public static int UserId { get; set; }
  }

  public class DiaryContext : DbContext
  {
    public DbSet<DiaryEntryDb> DiaryEntrys { get; set; }
    public DbSet<UserDb> Users { get; set; }
  }

  public static class DatabaseHelper
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
          foreach (var item in lstTestEntrys)
          {
            db.DiaryEntrys.Add(item);
          }
          db.SaveChanges();
        }
      }
    }
  }
}
