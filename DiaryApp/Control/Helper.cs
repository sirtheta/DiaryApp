using Notifications.Wpf.Core;
using System.Collections.Generic;
using System.Linq;

namespace DiaryApp.Control
{
  public class Helper
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

  public static class DatabaseHelper
  {
    //Adding the Tags  to DB if DB is empty
    public static void AddTags()
    {
      using (var db = new DiaryContext())
      {
        if (!db.Tags.Any())
        {
          db.Tags.Add(new TagDb { TagText = "Family" });
          db.Tags.Add(new TagDb { TagText = "Friends" });
          db.Tags.Add(new TagDb { TagText = "Birthday" });
          db.SaveChanges();
        }
      }
    }

    public static void CreateTestUser()//TODO
    {
      using (var db = new DiaryContext())
      {
        if (!db.Users.Any())
        {
          List<UserDb> lstUser = new List<UserDb>() { new UserDb { UserName = "Tester", FirstName = "me", LastName = "you", Password = "123456" } };
          db.Users.Add(lstUser[0]);
          db.SaveChanges();
        }
      }
    }
  }
}
