using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

/// <summary>
/// Testclass for the three enty manipulating methods
/// </summary>
namespace DiaryApp.Test
{
  [TestClass]
  public class ManipulateEntryTest
  {
    readonly MainWindowControl _mainWindowControl = new MainWindowControl();

    [TestMethod]
    public void SaveEntryTest()
    {
      _mainWindowControl.SignedInUserId = 4;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.EntryText = "SaveEntryTest";
      _mainWindowControl.CalendarSelectedDate = DateTime.Today.Date;
      _mainWindowControl.FamilyIsChecked = true;
      _mainWindowControl.FriendsIsChecked = false;
      _mainWindowControl.BirthdayIsChecked = true;
      _mainWindowControl.SaveEntry();
      var entry = DbController.GetEntrysFromDb(4).SingleOrDefault();
      Assert.AreEqual("SaveEntryTest", entry.Text);
      Assert.AreEqual(DateTime.Today.Date, entry.Date);
      Assert.IsTrue(entry.TagFamily);
      Assert.IsFalse(entry.TagFriends);
      Assert.IsTrue(entry.TagBirthday);
      Assert.AreEqual("Family, Birthday", entry.TagText);
    }

    [TestMethod]
    public void UpdateEntryTest()
    {
      _mainWindowControl.SignedInUserId = 3;
      _mainWindowControl.LoadEntrysFromDb();
      var entryToUpdate = DbController.GetEntrysFromDb(_mainWindowControl.SignedInUserId).SingleOrDefault();
      _mainWindowControl.EntryText = "Seed entry edited";
      _mainWindowControl.CalendarSelectedDate = DateTime.Today.AddDays(-1);
      _mainWindowControl.FamilyIsChecked = true;
      _mainWindowControl.BirthdayIsChecked = false;
      _mainWindowControl.FriendsIsChecked = true;
      _mainWindowControl.UpdateEntry(entryToUpdate);
      var entry = DbController.GetEntrysFromDb(3).SingleOrDefault();
      Assert.AreEqual("Seed entry edited", entry.Text);
      Assert.AreEqual(DateTime.Today.AddDays(-1).Date, entry.Date);
      Assert.IsTrue(entry.TagFamily);
      Assert.IsFalse(entry.TagBirthday);
      Assert.IsTrue(entry.TagFriends);
      Assert.AreEqual("Family, Friends", entry.TagText);
    }

    [TestMethod]
    public void DeleteEntryTest()
    {
      _mainWindowControl.SignedInUserId = 1;
      _mainWindowControl.LoadEntrysFromDb();
      var entryToDelete = DbController.GetEntrysFromDb(_mainWindowControl.SignedInUserId);
      _mainWindowControl.DeleteEntry(entryToDelete);
      Assert.AreNotSame(entryToDelete, DbController.GetEntrysFromDb(_mainWindowControl.SignedInUserId));
    }
  }
}
