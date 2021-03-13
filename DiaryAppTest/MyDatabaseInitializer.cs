using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace DiaryApp.Test
{
  internal class MyDatabaseInitializer : DropCreateDatabaseAlways<DiaryContext>
  {
    protected override void Seed(DiaryContext context)
    {
      var user1 = new UserModel { UserName = "Tester" };
      var user2 = new UserModel { UserName = "Tester2" };
      context.Users.Add(user1);
      context.Users.Add(user2);
      context.SaveChanges();
    }
  }
}
