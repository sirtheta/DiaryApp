using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DiaryApp.Control
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
