﻿using MaterialDesignMessageBox;
using Notifications.Wpf.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace DiaryApp
{
  //Baseclass for Control
  abstract class ControlBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool ShowMessageBox(string messageStr, MessageType type, MessageButtons buttons)
    {
      return (bool)new CustomMessageBox(messageStr, type, buttons).ShowDialog();
    }

    public void ShowNotification(string titel, string message, NotificationType type)
    {
      var notificationManager = new NotificationManager();
      notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
              areaName: "WindowArea");
    }

    //Converts the SecureString to a normal string
    protected internal string ToNormalString(SecureString input)
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
