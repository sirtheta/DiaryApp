using System.Data.Entity;

namespace DiaryApp
{
  class DiaryContext : DbContext
  {
    public DbSet<DiaryEntryModel> DiaryEntrys { get; set; }
    public DbSet<UserModel> Users { get; set; }
  }
}
