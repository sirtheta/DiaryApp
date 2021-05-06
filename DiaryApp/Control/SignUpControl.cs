using MaterialDesignMessageBoxSirTheta;
using Notifications.Wpf.Core;
using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Input;
//set all internals visible for the unittests
[assembly: InternalsVisibleTo("DiaryApp.Test", AllInternalsVisible = true)]

/// <summary>
/// Control Class for Sign up view
/// </summary>
namespace DiaryApp
{
  internal class SignUpControl : ControlBase
  {
    DbController db = new DbController();
    private string _lastName;
    private string _firstName;
    private string _userName;
    private SecureString _signInPassword;
    private SecureString _signInPasswordConfirm;

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

    public SecureString SignInPassword
    {
      get => _signInPassword;
      set
      {
        _signInPassword = value;
        OnPropertyChanged();
      }
    }

    public SecureString SignInPasswordConfirm
    {
      get => _signInPasswordConfirm;
      set
      {
        _signInPasswordConfirm = value;
        OnPropertyChanged();
      }
    }

    //this sets the registered action in view to close the sign up window
    public Action CloseAction { get; set; }

    public ICommand CloseApplicationCommand
    {
      get => new RelayCommand<object>(ExecuteCloseWindow);
    }
    private void ExecuteCloseWindow(object Parameter) => CloseAction();

    //**************************************************************************
    //Methods
    //**************************************************************************
    #region Methods
    private bool SignUpGUIHandler()
    {
      var retVal = false;
      if (!CheckUserDuplicate())
      {
        ShowMessageBox("User already exists!", MessageType.Error, MessageButtons.Ok);
        UserName = string.Empty;
      }
      else if (!CheckPasswordMatch())
      {
        ShowMessageBox("Passwords are not matching!", MessageType.Error, MessageButtons.Ok);
      }
      else if (!CheckPwdWithRegex())
      {
        ShowMessageBox("The entered password does not meet the requirements. " +
          "Requirements: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit " +
          "and 1 special character.", MessageType.Error, MessageButtons.Ok);
      }
      else
      {
        CloseAction();
        ShowNotification("Success", "Sign up successfull!", NotificationType.Success);
        retVal = true;
      }
      return retVal;
    }

    public void SignUp()
    {
      if (SignUpGUIHandler())
      {
        CreateNewUser();
      }
    }

    ///check if entered user already exist. -->Get a list of user with the entered username
    ///this list should have zero entries, then user does not exist
    internal bool CheckUserDuplicate()
    {
      return db.GetUserFromDb(UserName).Count == 0;
    }

    ///Check the entered password with regex
    ///Regex: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit and 1 special char
    internal bool CheckPwdWithRegex()
    {
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");
      if (!regex.IsMatch(ToNormalString(SignInPassword)))
      {
        return false;
      }
      return true;
    }

    //check if the two entered password matches
    internal bool CheckPasswordMatch()
    {
      if (SignInPassword == null || SignInPasswordConfirm == null)
      {
        return false;
      }
      if (ToNormalString(SignInPassword) != ToNormalString(SignInPasswordConfirm))
      {
        return false;
      }
      //return true if every check passes
      return true;
    }

    internal void CreateNewUser()
    {
      var newUser = new UserModel()
      {
        LastName = LastName,
        FirstName = FirstName,
        UserName = UserName,
        Password = SecurePasswordHasher.Hash(ToNormalString(SignInPassword))
      };
      db.UserToDb(newUser);
    }
    #endregion
  }
}


