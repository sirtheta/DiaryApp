using System.Data.Entity;
using System.Windows;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      //If Database model has changed, drop the table and create an empty db
      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DiaryContext>());
      DatabaseInitializer dbIni = new DatabaseInitializer();
      dbIni.CreateTestUser();
      dbIni.CreateTestEntrys();
    }
  }
}
