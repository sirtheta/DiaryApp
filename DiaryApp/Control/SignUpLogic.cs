﻿using Notifications.Wpf.Core;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiaryApp
{
  class SignUpLogic : AbstractPropertyChanged
  {
    readonly Model model = new Model();

    private string _lastName;
    private string _firstName;
    private string _userName;
    private SecureString _password;
    private SecureString _passwordConfirm;

    public string LastName
    {
      get => _lastName;
      set
      {
        _lastName = value;
        OnPropertyChanged();
      }
    }

    public string FirstName
    {
      get => _firstName;
      set
      {
        _firstName = value;
        OnPropertyChanged();
      }
    }

    public string UserName
    {
      get => _userName;
      set
      {
        _userName = value;
        OnPropertyChanged();
      }
    }

    public SecureString Password
    {
      get => _password;
      set
      {
        _password = value;
        OnPropertyChanged();
      }
    }

    public SecureString PasswordConfirm
    {
      get => _passwordConfirm;
      set
      {
        _passwordConfirm = value;
        OnPropertyChanged();
      }
    }

    public bool SignUp()
    {
      var iUser = model.GetUser(UserName).Count;
      if (iUser == 0 && CheckPassword())
      {
        CreateNewUser();
        Helper.ShowNotification("Success", "Sign up successfull!", NotificationType.Success);
        return true;
      }
      else if (iUser > 0)
      {
        Helper.ShowMessageBox("User already exists!", MessageType.Error, MessageButtons.Ok);
        UserName = string.Empty;
        return false;
      }
      else return false;
    }

    private void CreateNewUser()
    {
      var newUser = new UserDb()
      {
        LastName = LastName,
        FirstName = FirstName,
        UserName = UserName,
        Password = SecurePasswordHasher.Hash(Helper.ToNormalString(Password))
      };
      model.UserToDb(newUser);
    }

    private bool CheckPassword()
    {
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");
      if (Password == null || PasswordConfirm == null)
      {
        return false;
      }
      if (Helper.ToNormalString(Password) != Helper.ToNormalString(PasswordConfirm))
      {
        Helper.ShowMessageBox("Passwords are not matching!", MessageType.Error, MessageButtons.Ok);
        Password = null;
        PasswordConfirm = null;
        return false;
      }
      if (!regex.IsMatch(Helper.ToNormalString(Password)))
      {
        Helper.ShowMessageBox("The entered password does not meet the requirements. Requirements: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit and 1 special character.", MessageType.Error, MessageButtons.Ok);
        Password = null;
        PasswordConfirm = null;
        return false;
      }
      return true;
    }
  }
}


