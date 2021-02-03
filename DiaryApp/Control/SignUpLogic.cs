using System.Text.RegularExpressions;

namespace DiaryApp
{
  class SignUpLogic
  {
    string UserName;
    string Password;
    string Salt;
    string LastName;
    string FirstName;

    public bool CheckPassword()
    {
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");

      return true;
    }
  }
}
