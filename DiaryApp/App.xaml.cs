using DiaryApp.Control;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
      //If Database propertys has changed, drop the table and create an empty db
      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DiaryContext>());
      DatabaseHelper.AddTags();
      DatabaseHelper.CreateTestUser();
    }
  }
}
