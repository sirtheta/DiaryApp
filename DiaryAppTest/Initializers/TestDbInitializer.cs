using System;
using System.Data.Entity;

namespace DiaryApp.Test
{
  internal class TestDbInitializer : DropCreateDatabaseAlways<DiaryContext>
  {
    protected override void Seed(DiaryContext context)
    {
      AddTestUser(context);
      AddTestEntrys(context);
    }

    private void AddTestUser(DiaryContext context)
    {
      context.Users.Add(new UserModel { UserName = "Tester" });
      context.Users.Add(new UserModel { UserName = "Tester2" });
      context.Users.Add(new UserModel { FirstName = "John", LastName = "Doe", UserName = "SirJohnDoe", Password = SecurePasswordHasher.Hash("5T!kq*o4f") });
      context.Users.Add(new UserModel { FirstName = "Michelle", LastName = "Hunziker", UserName = "Michelle", Password = SecurePasswordHasher.Hash("Cr75@O&Hz") });
      context.SaveChanges();
    }

    private void AddTestEntrys(DiaryContext context)
    {
      //Entry to delete
      context.DiaryEntrys.Add(new DiaryEntryModel { Text = "Entry to delete", Date = DateTime.Today.Date, TagBirthday = true, TagFamily = false, TagFriends = false, UserId = 1 });
      //Entrys to test SearchDateWithoutEntry()
      context.DiaryEntrys.Add(new DiaryEntryModel { Text = "Entry for SearchDateWithoutEntryTest", Date = DateTime.Today.Date, TagBirthday = true, TagFamily = false, TagFriends = false, UserId = 2 });
      context.DiaryEntrys.Add(new DiaryEntryModel { Text = "Entry for SearchDateWithoutEntryTest", Date = DateTime.Today.Date, TagBirthday = true, TagFamily = false, TagFriends = false, UserId = 2 });
      context.DiaryEntrys.Add(new DiaryEntryModel { Text = "Entry for SearchDateWithoutEntryTest", Date = DateTime.Today.Date, TagBirthday = true, TagFamily = false, TagFriends = false, UserId = 2 });
      //Entry to update
      context.DiaryEntrys.Add(new DiaryEntryModel { Text = "Seed entry", Date = DateTime.Today.Date, TagBirthday = true, TagFamily = false, TagFriends = false, UserId = 3 });
      context.SaveChanges();
    }
  }
}
