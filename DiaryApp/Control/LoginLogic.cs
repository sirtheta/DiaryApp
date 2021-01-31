using System.Linq;
using System.Windows;

namespace DiaryApp.Control
{
  class LoginLogic
  {
    string userName;
    string password;

    public LoginLogic(string userName, string password)
    {
      this.userName = userName;
      this.password = password;
    }

    private bool CheckForValidUser()
    {
      using (var db = new DiaryContext())
      {
        if (db.Users.Any(o => o.UserName == userName) && db.Users.Any(o => o.Password == password))
        {
          //Set userID to use in MainWindowLogic
          Globals.UserId = (from b in db.Users
                            where b.UserName == userName
                            select b.UserId).SingleOrDefault();
          return true;
        }
      }
      return false;
    }

    public bool Login()
    {

      if (CheckForValidUser())
      {
        Window main = new MainWindow();
        main.Show();
        return true;
      }
      else
      {
        Helper.ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
        return false;
      }
    }
  }
}
