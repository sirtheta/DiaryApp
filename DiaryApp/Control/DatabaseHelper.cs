using System.Collections.Generic;
using System.Linq;

namespace DiaryApp
{

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
