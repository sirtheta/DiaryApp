using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;

namespace DiaryApp.Test
{
  [TestClass()]
  public class SignUpControlTests
  {

    SignUpControl _signUpControl = new SignUpControl();

    [TestMethod("Password Regex Test true")]
    public void CheckPwdWithRegexTest()
    {      
      _signUpControl.SignInPassword = ConvertToSecureString("5T!kq*o4f");
      Assert.IsTrue(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no special char")]
    public void CheckPwdWithRegexTestFalse()
    {
      _signUpControl.SignInPassword = ConvertToSecureString("5Tkqo4f89");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: not enough char")]
    public void CheckPwdWithRegexTestFalseNSpecial()
    {
      _signUpControl.SignInPassword = ConvertToSecureString("5Tkqo4*");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Regex Test: no lower char")]
    public void CheckPwdWithRegexTestFalseNLower()
    {
      _signUpControl.SignInPassword = ConvertToSecureString("5TKOP4*9");
      Assert.IsFalse(_signUpControl.CheckPwdWithRegex());
    }

    [TestMethod("Password Match Test: true")]   
    public void CheckPasswordMatchTestTrue()
    {
      _signUpControl.SignInPassword = ConvertToSecureString("5T!kq*o4f");
      _signUpControl.SignInPasswordConfirm = ConvertToSecureString("5T!kq*o4f");
      Assert.IsTrue(_signUpControl.CheckPasswordMatch());
    }

    [TestMethod("Password Match Test: false")]
    public void CheckPasswordMatchTestFalse()
    {
      _signUpControl.SignInPassword = ConvertToSecureString("5T!kq*o4f");
      _signUpControl.SignInPasswordConfirm = ConvertToSecureString("5T!kqo*4f");
      Assert.IsFalse(_signUpControl.CheckPasswordMatch());
    }

    //TODO, Test with DB.
    [TestMethod("User Duplicate: new user")]
    public void CheckUserDuplicateTest()
    {
      PrepareData();
      _signUpControl.UserName = "gibberish";
      Assert.IsTrue(_signUpControl.CheckUserDuplicate());
    }

    [TestMethod("User Duplicate: existing user")]
    public void CheckUserDuplicateTestFalse()
    {
      PrepareData();
      _signUpControl.UserName = "Tester";
      Assert.IsFalse(_signUpControl.CheckUserDuplicate());
    }

    private SecureString ConvertToSecureString(string password)
    {
      if (password == null)
        throw new ArgumentNullException("password");

      var securePassword = new SecureString();

      foreach (char c in password)
        securePassword.AppendChar(c);

      securePassword.MakeReadOnly();
      return securePassword;
    }

    private void PrepareData()
    {
      using (var model = new DiaryContext())
      {
        var user1 = new UserModel { UserName = "Tester" };
        var user2 = new UserModel { UserName = "Tester2" };
        model.Users.Add(user1);
        model.Users.Add(user2);
        model.SaveChanges();
      }
    }

  }
}