using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Testclass to test all the password requirements
/// </summary>
namespace DiaryApp.Test
{
  [TestClass]
  public class SignUpControlPasswordTests
  {
    readonly SignUpControl _signUpControl = new SignUpControl();

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
  }
}