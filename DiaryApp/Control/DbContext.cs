﻿using System.Data.Entity;

namespace DiaryApp.Control
{
  public class DiaryContext : DbContext
  {
    public DbSet<DiaryEntryDb> DiaryEntrys { get; set; }
    public DbSet<UserDb> Users { get; set; }
    public DbSet<TagDb> Tags { get; set; }
  }
}
