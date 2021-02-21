﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace DiaryApp
{
  class DiaryEntryDb
  {
    [Key]
    public int EntryId { get; set; }
    public string Text { get; set; }
    public bool TagFamily { get; set; }
    public bool TagFriends { get; set; }
    public bool TagBirthday { get; set; }
    public DateTime Date { get; set; }
    public byte[] ByteImage { get; set; }

    //Add foreignkey from UserDb to Entry
    public int UserId { get; set; }
    public virtual UserDb UserDb { get; set; }

    public string TagText
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        if (TagFamily)
        {
          sb.Append("Family");
        }
        if (TagFriends)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Friends");
        }
        if (TagBirthday)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Birthday");
        }
        return Regex.Replace(sb.ToString(), "[^A-Za-z0-9, ]", "");
      }
    }
  }
}
