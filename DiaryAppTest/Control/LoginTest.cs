using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DiaryApp.Test
{
  [TestClass]
  public class LoginTest
  {
    readonly MainWindowControl _mainWindowControl = new MainWindowControl();

    [TestMethod]
    public void UserVerificationTest()
    {
      var user = DbController.GetUserFromDb("Michelle").SingleOrDefault();
      _mainWindowControl.Verified(user);
      Assert.AreEqual("Michelle Hunziker", _mainWindowControl.SignedInUserFullName);
    }
  }
}
