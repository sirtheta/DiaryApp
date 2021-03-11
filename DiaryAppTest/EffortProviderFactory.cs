using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace DiaryApp.Test
{
  public class EffortProviderFactory : IDbConnectionFactory
  {
    private static DbConnection _connection;
    private readonly static object _lock = new object();

    public static void ResetDb()
    {
      lock (_lock)
      {
        _connection = null;
      }
    }

    public DbConnection CreateConnection(string nameOrConnectionString)
    {
      lock (_lock)
      {
        if (_connection == null)
        {
          _connection = Effort.DbConnectionFactory.CreateTransient();
        }

        return _connection;
      }
    }
  }
}
