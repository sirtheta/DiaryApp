using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Notifications.Wpf.Core;
using System.Data.Entity;

namespace DiaryApp.Control
{
  class Logic
  {
    string entryText;
    DateTime date;
    bool tagFamily;
    bool tagFriends;
    bool tagBirthday;
    DiaryEntryDb entrys;

    List<DiaryEntryDb> lstEntry = new List<DiaryEntryDb>();
    List<TagDb> lstTag = new List<TagDb>();

    #region getter
    public List<TagDb> LstTag
    {
      get
      {
        using (var db = new DiaryContext())
        {
          var query = from b in db.Tags
                      orderby b.TagID
                      select b;

          foreach (var item in query)
          {
            lstTag.Add(item);
          }
        }
        return lstTag;
      }
    }

    public List<DiaryEntryDb> LstEntry
    {
      get
      {
        using (var db = new DiaryContext())
        {
          var query = from b in db.DiaryEntrys
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

    //TODO
    public List<DiaryEntryDb> LstEntryByTag
    {
      get
      {        
        using (var db = new DiaryContext())
        {
          var query = from b in db.DiaryEntrys
                      where b.Tag.Contains("Family")
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

    public List<DiaryEntryDb> LstEntryByDate
    {
      get
      {
        var strDate = date.ToString("dd. MMMM yyyy");
        using (var db = new DiaryContext())
        {
          var query = from b in db.DiaryEntrys
                      where b.Date == strDate
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
#endregion getter

    #region constructor
    //empty constructor for initialization
    public Logic()
    {
    }

    //empty constructor to search for tags
    public Logic(bool t_tagFamily, bool t_tagFriends, bool t_tagBirthday)
    {
      tagFamily = t_tagFamily;
      tagFriends = t_tagFriends;
      tagBirthday = t_tagBirthday;
    }

    //empty constructor to search by date
    public Logic(DateTime t_date)
    {
      date = t_date;
    }

    //Constructor for deletion
    public Logic(DiaryEntryDb t_entrys)
    {
      entrys = t_entrys;
    }


    //constructor to add entrys
    public Logic(string t_entryText, bool t_tagFamily, bool t_tagFriends, bool t_tagBirthday, DateTime t_date)
    {
      entryText = t_entryText;
      tagFamily = t_tagFamily;
      tagFriends = t_tagFriends;
      tagBirthday = t_tagBirthday;
      date = t_date;
    }
    #endregion constructor

    #region methods
    public string CreateTagText()
    {
      StringBuilder sb = new StringBuilder();
      if (tagFamily == true)
      {
        sb.Append(LstTag[0].TagText);
      }
      if (tagFriends == true)
      {
        if (sb.Length != 0)
        {
          sb.Append(", ");
        }
        sb.Append(LstTag[1].TagText);
      }
      if (tagBirthday == true)
      {
        if (sb.Length != 0)
        {
          sb.Append(", ");
        }
        sb.Append(LstTag[2].TagText);
      }
      return Regex.Replace(sb.ToString(), "[^A-Za-z0-9, ]", "");
    }

    public List<DiaryEntryDb> SaveEntry()
    {
      var strDate = date.ToString("dd. MMMM yyyy");

      string tag = CreateTagText();
      if (entryText != "")
      {
        using (var db = new DiaryContext())
        {
          var newEntry = new DiaryEntryDb() { Text = entryText, Date = strDate, Tag = tag };
          db.DiaryEntrys.Add(newEntry);
          db.SaveChanges();
        }

        Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
      }
      else
      {
        Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
      }
      return LstEntry;
    }

    
    public List<DiaryEntryDb> DeleteSelectedEntry()
    {
      if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
      {
        lstEntry.Remove(entrys);
        using (var db = new DiaryContext())
        {
          db.Entry(entrys).State = EntityState.Deleted;
          db.SaveChanges();
        }
        Helper.ShowNotification("Success", "Diary Entry Successfull deleted", NotificationType.Success);
      }
      return LstEntry;
    }
  }
}
#endregion