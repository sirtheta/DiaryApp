using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

/// <summary>
/// Methods in this class are called at beginning and at the end of test
/// </summary>
namespace DiaryApp.Test
{
  [TestClass]
  public class AssemblyControl
  {
    //Is run once when test is started,
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
      Database.SetInitializer(new TestDbInitializer());
      Helper.InitializeTestingDb();
    }

    //is running after all test has finished
    [AssemblyCleanup]
    public static void Cleanup()
    {
      //Drop testing DB
      using var deleteModel = new DiaryContext();
      deleteModel.Database.Delete();
    }
  }
}
