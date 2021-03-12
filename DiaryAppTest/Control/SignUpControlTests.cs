using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security;

namespace DiaryApp.Test
{
  [TestClass]
  public class SignUpControlTests
  {
    //Is run once when test is started,
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
      //Drop existing DB
      using (var deleteModel = new DiaryContext())
      {
        deleteModel.Database.Delete();
      }
      //Create user in empty DB
      using var model = new DiaryContext();
      var user1 = new UserModel { UserName = "Tester" };
      var user2 = new UserModel { UserName = "Tester2" };
      model.Users.Add(user1);
      model.Users.Add(user2);
      model.SaveChanges();
    }

    //is running after all test has finished
    [AssemblyCleanup]
    public static void Cleanup()
    {
      //Drop testing DB
      using (var deleteModel = new DiaryContext())
      {
        deleteModel.Database.Delete();
      }
    }

    SignUpControl _signUpControl = new SignUpControl();

    [TestMethod("Password Regex Test true")]
    public void CheckPwdWithRegexTest()
    {      
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5T!kq*o4f");
      Assert.IsTrue(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no special char")]
    public void CheckPwdWithRegexTestFalseNoSpecial()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5Tkqo4f89");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: not enough char")]
    public void CheckPwdWithRegexTestFalseNotEnough()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5Tkqo4*");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no lower char")]
    public void CheckPwdWithRegexTestFalseNoLower()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5TKOP4*9");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no upper char")]
    public void CheckPwdWithRegexTestFalseNoUpper()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5tkop4*9");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no numbers")]
    public void CheckPwdWithRegexTestFalseNoNumbers()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("TtkopI*p");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Match Test: true")]   
    public void CheckPasswordMatchTestTrue()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5T!kq*o4f");
      _signUpControl.SignInPasswordConfirm = Helper.ConvertToSecureString("5T!kq*o4f");
      Assert.IsTrue(_signUpControl.CheckPasswordMatch());
    }

    [TestMethod("Password Match Test: false")]
    public void CheckPasswordMatchTestFalse()
    {
      _signUpControl.SignInPassword = Helper.ConvertToSecureString("5T!kq*o4f");
      _signUpControl.SignInPasswordConfirm = Helper.ConvertToSecureString("5T!kqo*4f");
      Assert.IsFalse(_signUpControl.CheckPasswordMatch());
    }

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
      var user = DbController.GetUserFromDb("JDO").SingleOrDefault();
      Assert.AreEqual("John", user.FirstName);
      Assert.AreEqual("Doe", user.LastName);
      Assert.AreEqual("JDO", user.UserName);
      Assert.IsTrue(SecurePasswordHasher.Verify("5T!kq*o4f", user.Password));
    }
  }
}