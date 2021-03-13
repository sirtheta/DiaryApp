using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace DiaryApp.Test
{
  [TestClass]
  public class SignUpControlCreateUserTest
  {
    //Is run once when test is started,
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
      Database.SetInitializer(new MyDatabaseInitializer());
      Helper.InitializeUserDb();
    }

    //is running after all test has finished
    [AssemblyCleanup]
    public static void Cleanup()
    {
      //Drop testing DB
      using var deleteModel = new DiaryContext();
      deleteModel.Database.Delete();
    }

    readonly SignUpControl _signUpControl = new SignUpControl();

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
