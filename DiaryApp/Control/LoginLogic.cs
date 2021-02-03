using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DiaryApp.Control
{
  class LoginLogic
  {

    private bool CheckForValidUser(string userName, string password)
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

    public bool Login(TextBox userName, PasswordBox password)
    {

      if (CheckForValidUser(userName.Text, password.Password))
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
