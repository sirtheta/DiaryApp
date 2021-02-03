using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DiaryApp.Control
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
  }

  static class Globals
  {
    public static int UserId { get; set; }
  }
}
