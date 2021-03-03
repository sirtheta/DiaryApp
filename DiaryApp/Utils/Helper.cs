using MaterialDesignMessageBox;
using Notifications.Wpf.Core;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DiaryApp
{
  class Helper
  {
    //Converts the SecureString to a normal string
    public static string ToNormalString(SecureString input)
    {
      IntPtr strptr = IntPtr.Zero;
      try
      {
        strptr = Marshal.SecureStringToBSTR(input);
        string normal = Marshal.PtrToStringBSTR(strptr);
        return normal;
      }
      catch
      {
        return null;
      }
      finally
      {
        //Free the pointer holding the SecureString
        Marshal.ZeroFreeBSTR(strptr);
      }
    }
  }
}
