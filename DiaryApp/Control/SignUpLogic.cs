using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace DiaryApp
{
  class SignUpLogic
  {
    readonly Model model = new Model();

    TextBox txtBoxLastName;
    TextBox txtBoxFirstName;
    TextBox txtBoxUserName;
    PasswordBox txtBoxPassword;
    PasswordBox txtBoxPasswordConfirm;

    public SignUpLogic() { }

    public SignUpLogic(TextBox t_txtBoxLastName,
                       TextBox t_txtBoxFirstName,
                       TextBox t_txtBoxUserName,
                       PasswordBox t_txtBoxPassword,
                       PasswordBox t_txtBoxPasswordConfirm)
    {
      txtBoxLastName = t_txtBoxLastName;
      txtBoxFirstName = t_txtBoxFirstName;
      txtBoxUserName = t_txtBoxUserName;
      txtBoxPassword = t_txtBoxPassword;
      txtBoxPasswordConfirm = t_txtBoxPasswordConfirm;
    }

    public bool SignUp()
    {
      try
      {
        model.GetUser(txtBoxUserName.Text).Single();

        if (CheckPassword())
        {
          CreateNewUser();
          return true;
        }
        return false;
      }
      catch (Exception)
      {
        Helper.ShowMessageBox("User already exists!", MessageType.Error, MessageButtons.Ok);
        txtBoxUserName.Text = "";
        return false;
      }
    }

    private void CreateNewUser()
    {
      var newUser = new UserDb()
      {
        LastName = txtBoxLastName.Text,
        FirstName = txtBoxFirstName.Text,
        UserName = txtBoxUserName.Text,
        Password = SecurePasswordHasher.Hash(txtBoxPassword.Password)
      };
      model.UserToDb(newUser);
    }

    private bool CheckPassword()
    {
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");
      if (txtBoxPassword.Password == txtBoxPasswordConfirm.Password)
      {
        Helper.ShowMessageBox("Passwords are not matching!", MessageType.Error, MessageButtons.Ok);
        txtBoxPassword = null;
        txtBoxPasswordConfirm = null;
        return false;
      }
      if (!regex.IsMatch(txtBoxPassword.Password))
      {
        Helper.ShowMessageBox("The entered password does not meet the requirements. Requirements: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit and 1 special character.", MessageType.Error, MessageButtons.Ok);
        txtBoxPassword = null;
        txtBoxPasswordConfirm = null;
        return false;
      }
      return true;
    }
  }
}

