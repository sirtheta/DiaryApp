﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DiaryApp.Test
{
  [TestClass]
  public class LoginTest
  {
    readonly MainWindowControl _mainWindowControl = new MainWindowControl();

    [TestMethod]
    public void VerifiedTest()
    {
      var user = DbController.GetUserFromDb("Michelle").SingleOrDefault();
      _mainWindowControl.Verified(user);
      Assert.AreEqual("Michelle Hunziker", _mainWindowControl.SignedInUserFullName);
    }

    [TestMethod]
    public void VerifyUserTestCorrectPassword()
    {
      _mainWindowControl.SignInUserName = "Michelle";
      _mainWindowControl.SignInPassword = Helper.ConvertToSecureString("Cr75@O&Hz");
      Assert.IsTrue(_mainWindowControl.VerifyUser());
    }

    [TestMethod]
    public void VerifyUserTestWrongPassword()
    {
      _mainWindowControl.SignInUserName = "Michelle";
      _mainWindowControl.SignInPassword = Helper.ConvertToSecureString("5T!kqo*4f");
      Assert.IsFalse(_mainWindowControl.VerifyUser());
    }

    [TestMethod]
    public void VerifyUserTestNoPassword()
    {
      _mainWindowControl.SignInUserName = "SirJohnDoe";
      Assert.IsFalse(_mainWindowControl.VerifyUser());
    }
  }
}
