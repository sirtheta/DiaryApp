using System.Data.Entity;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace DiaryApp
{
  internal class DiaryContext : DbContext
  {
    public virtual DbSet<DiaryEntryModel> DiaryEntrys { get; set; }
    public virtual DbSet<UserModel> Users { get; set; }
  }
}
