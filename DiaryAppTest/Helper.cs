using System;
using System.Security;


/// <summary>
/// Helperclass for UnitTests
/// </summary>
namespace DiaryApp.Test
{
  internal static class Helper
  {
    //Convert string to Securestring as only secure strings are acepted in coresponding properties
    public static SecureString ConvertToSecureString(string password)
    {
      if (password == null)
        throw new ArgumentNullException("password");

      var securePassword = new SecureString();

      foreach (char c in password)
        securePassword.AppendChar(c);

      securePassword.MakeReadOnly();
      return securePassword;
    }

    //Sets the DB Context for Testing Database
    public static void InitializeTestingDb()
    {
      DiaryContext.ConnectionName = "DiaryAppTest";
      DiaryContext context = new DiaryContext();
      context.Database.Initialize(true);
    }
  }
}
