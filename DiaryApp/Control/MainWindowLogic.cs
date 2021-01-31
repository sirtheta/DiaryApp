using DiaryApp.Model;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace DiaryApp.Control
{
  class MainWindowLogic
  {
    readonly List<DiaryEntryDb> lstEntry = new List<DiaryEntryDb>();
    readonly List<string> lstTag = new List<string>() { "Family", "Friends", "Birthday" };
    string selectedFileName;
    int userId = 1;

    public int UserId { get; set; }

    #region getter
    public List<DiaryEntryDb> LstEntry
    {
      get
      {
        using (var db = new DiaryContext())
        {
          var query = from b in db.DiaryEntrys
                      where b.UserId == userId
                      orderby b.EntryId
                      select b;

          foreach (var item in query)
          {
            lstEntry.Add(item);
          }
        }
        return lstEntry;
      }
    }

    public List<string> LstTag
    {
      get
      {
        return lstTag;
      }
    }
    #endregion getter

    #region SaveDelete Entry
    //Save the Created Entry to Database
    public List<DiaryEntryDb> SaveEntry(string entryText, bool tagFamily, bool tagFriends, bool tagBirthday, DateTime date)
    {
      //Convert image to byteArray
      if (entryText != "")
      {
        using (var db = new DiaryContext())
        {
          var newEntry = new DiaryEntryDb()
          {
            Text = entryText,
            Date = date.ToString("dd. MMMM yyyy"),
            TagFamily = tagFamily,
            TagFriends = tagFriends,
            TagBirthday = tagBirthday,
            ByteImage = ImageToByteArray(),
            UserId = userId
          };
          db.DiaryEntrys.Add(newEntry);
          db.SaveChanges();
          lstEntry.Add(newEntry);
        }
        Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
      }
      else
      {
        Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
      }
      return lstEntry;
    }

    public byte[] ImageToByteArray()
    {
      if (selectedFileName != null)
      {
        return File.ReadAllBytes(selectedFileName);
      }
      return null;
    }

    //Delete selected entry from Database
    public List<DiaryEntryDb> DeleteSelectedEntry(DiaryEntryDb entrys)
    {
      if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
      {
        lstEntry.Remove(entrys);
        using (var db = new DiaryContext())
        {
          db.Entry(entrys).State = EntityState.Deleted;
          db.SaveChanges();
          lstEntry.Remove(entrys);
        }
        Helper.ShowNotification("Success", "Diary Entry Successfull deleted", NotificationType.Success);
      }
      return lstEntry;
    }

    public BitmapImage AddImage()
    {
      //Add Image
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
      if (dialog.ShowDialog() == true)
      {

        selectedFileName = dialog.FileName;
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(selectedFileName);
        bitmap.EndInit();
        return bitmap;
      }
      return null;
    }
    #endregion

    #region Search
    //Search for entrys by Date
    public List<DiaryEntryDb> GetEntrysByDate(DateTime date)
    {
      return lstEntry.Where(lst => lst.Date == date.ToString("dd. MMMM yyyy")).ToList();
    }

    public List<DiaryEntryDb> ShowAll()
    {
      return lstEntry;
    }

    //Search for entrys by Tag
    public List<DiaryEntryDb> GetEntrysByTag(bool tagFamily, bool tagFriends, bool tagBirthday)
    {
      if (tagFamily == true && tagFriends == false && tagBirthday == false)
      {
        return lstEntry.Where(lst => lst.TagFamily).ToList();
      }
      else if (tagFamily == false && tagFriends == true && tagBirthday == false)
      {
        return lstEntry.Where(lst => lst.TagFriends).ToList();
      }
      else if (tagFamily == false && tagFriends == false && tagBirthday == true)
      {
        return lstEntry.Where(lst => lst.TagBirthday).ToList();
      }
      else if (tagFamily == true && tagFriends == true && tagBirthday == false)
      {
        return lstEntry.Where(lst => lst.TagFamily && lst.TagFriends).ToList();
      }
      else if (tagFamily == true && tagFriends == true && tagBirthday == true)
      {
        return lstEntry.Where(lst => lst.TagFamily && lst.TagBirthday && lst.TagFriends).ToList();
      }
      else if (tagFamily == true && tagFriends == false && tagBirthday == true)
      {
        return lstEntry.Where(lst => lst.TagFamily && lst.TagBirthday).ToList();
      }
      else if (tagFamily == false && tagFriends == true && tagBirthday == true)
      {
        return lstEntry.Where(lst => lst.TagFriends && lst.TagBirthday).ToList();
      }
      else
      {
        return lstEntry;
      }
    }
    #endregion
  }
}
