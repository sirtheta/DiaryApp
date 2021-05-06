using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

/// <summary>
/// Testclass to create new user
/// </summary>
namespace DiaryApp.Test
{
  [TestClass]
  public class SignUpControlCreateUserTest
  {
    readonly SignUpControl _signUpControl = new SignUpControl();
    DbController db = new DbController();
    [TestMethod("User Duplicate: new user")]
    public void CheckUserDuplicateTest()
    {
      _signUpControl.UserName = "gibberish";
      Assert.IsTrue(_signUpControl.CheckUserDuplicate());
    }

    [TestMethod("User Duplicate: existing user")]
    public void CheckUserDuplicateTestFalse()
    {
      _signUpControl.UserName = "Tester";
      Assert.IsFalse(_signUpControl.CheckUserDuplicate());
    }

    [TestMethod("Create new User")]
    public void TestCreateNewUser()
    {
      _signUpControl.FirstName = "John";
      _signUpControl.LastName = "Doe";
      _signUpControl.UserName = "JDO";
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5T!kq*o4f");
      _signUpControl.CreateNewUser();
      var user = db.GetUserFromDb("JDO").SingleOrDefault();
      Assert.AreEqual("John", user.FirstName);
      Assert.AreEqual("Doe", user.LastName);
      Assert.AreEqual("JDO", user.UserName);
      Assert.IsTrue(SecurePasswordHasher.Verify("5T!kq*o4f", user.Password));
    }
  }
}
