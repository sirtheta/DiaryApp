using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Testclass for all search related methods
/// </summary>
namespace DiaryApp.Test
{
  [TestClass]
  public class SearchTest
  {
    readonly MainWindowControl _mainWindowControl = new MainWindowControl();

    [TestMethod]
    public void GetDatesWithoutEntryTest()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.CalendarSelectedRange = DateRange();
      Assert.IsTrue(_mainWindowControl.GetDatesWithoutEntry());
      Assert.AreEqual(8, _mainWindowControl.EntriesToShow.Count());
    }

    [TestMethod]
    public void GetDatesWithoutEntryTestFalse()
    {
      IList dateRangeNotSet = new List<DateTime>
      {
        DateTime.Today
      };
      _mainWindowControl.CalendarSelectedRange = dateRangeNotSet;
      Assert.IsFalse(_mainWindowControl.GetDatesWithoutEntry());
    }

    [TestMethod]
    public void GetEntrysByTagTestOne()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FamilyIsChecked = true;
      Assert.AreEqual(3, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestTwo()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.BirthdayIsChecked = true;
      Assert.AreEqual(3, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestThree()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FriendsIsChecked = true;
      Assert.AreEqual(2, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestFour()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FamilyIsChecked = true;
      _mainWindowControl.BirthdayIsChecked = true;
      Assert.AreEqual(4, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestFive()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FamilyIsChecked = true;
      _mainWindowControl.BirthdayIsChecked = true;
      _mainWindowControl.FriendsIsChecked = true;
      Assert.AreEqual(5, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestSix()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FamilyIsChecked = false;
      _mainWindowControl.BirthdayIsChecked = false;
      _mainWindowControl.FriendsIsChecked = false;
      Assert.AreEqual(1, _mainWindowControl.GetEntrysByTag().Count());
    }

    [TestMethod]
    public void GetEntrysByTagTestSeven()
    {
      _mainWindowControl.SignedInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.FriendsIsChecked = true;
      _mainWindowControl.BirthdayIsChecked = true;
      Assert.AreEqual(5, _mainWindowControl.GetEntrysByTag().Count());
    }

    private IList DateRange()
    {
      IList dateRange = new List<DateTime>
      {
        DateTime.Today,
        DateTime.Today.AddDays(-1),
        DateTime.Today.AddDays(-2),
        DateTime.Today.AddDays(-3),
        DateTime.Today.AddDays(-4),
        DateTime.Today.AddDays(-5),
        DateTime.Today.AddDays(-6),
        DateTime.Today.AddDays(-7),
        DateTime.Today.AddDays(-8)
      };
      return dateRange;
    }
  }
}
