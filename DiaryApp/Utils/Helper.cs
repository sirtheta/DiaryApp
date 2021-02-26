using Notifications.Wpf.Core;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DiaryApp
{
  class Helper
  {
    public static bool ShowMessageBox(string messageStr, MessageType type, MessageButtons buttons)
    {
      return (bool)new CustomMessageBox(messageStr, type, buttons).ShowDialog();
    }

    public static void ShowNotification(string titel, string message, NotificationType type)
    {
      var notificationManager = new NotificationManager();
      notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
              areaName: "WindowArea");
    }

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
        ShowMessageBox("Something went wrong. Try again", MessageType.Error, MessageButtons.Ok);
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
