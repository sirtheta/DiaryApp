using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiaryApp.Test
{
  [TestClass]
  public class SearchTest
  {
    readonly MainWindowControl _mainWindowControl = new MainWindowControl();

    [TestMethod]
    public void SearchDateWithoutEntryTest()
    {
      _mainWindowControl.SigneInUserId = 2;
      _mainWindowControl.LoadEntrysFromDb();
      _mainWindowControl.CalendarSelectedRange = DateRange();
      Assert.AreEqual(8, _mainWindowControl.GetDatesWithoutEntry().Count());
    }

    [TestMethod]
    public void SearchDateWithoutEntryException()
    {
      IList dateRangeNotSet = new List<DateTime>
      {
        DateTime.Today
      };
      _mainWindowControl.CalendarSelectedRange = dateRangeNotSet;
      try
      {
        _mainWindowControl.ShowDatesWithoutEntry();
        Assert.Fail();
      }
      catch (Exception)
      {
        Console.WriteLine("Exception thrown, Test passed!");
      }
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
