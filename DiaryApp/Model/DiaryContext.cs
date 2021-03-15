﻿using System.Data.Entity;

namespace DiaryApp
{
  class DiaryContext : DbContext
  {
    private static string _connectionName;
    ///sets the connection name configured in App.config
    ///if ConnectionName is not set, the standard connection "DiaryApp" will be used
    public static string ConnectionName
    {
      get
      {
        if (_connectionName == null)
        {
          return "DiaryApp";
        }
        else
        {
          return _connectionName;
        }
      }

      set
      {
        _connectionName = value;
      }
    }

    public DiaryContext() : base(ConnectionName)
    {
    }
    public virtual DbSet<DiaryEntryModel> DiaryEntrys { get; set; }
    public virtual DbSet<UserModel> Users { get; set; }
  }
}
