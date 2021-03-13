using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace DiaryApp.Test
{
  internal static class Helper
  {
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
    public static void InitializeUserDb()
    {
      DiaryContext.ConnectionName = "DiaryAppTest";
      DiaryContext context = new DiaryContext();
      context.Database.Initialize(true);
    }
  }
}
