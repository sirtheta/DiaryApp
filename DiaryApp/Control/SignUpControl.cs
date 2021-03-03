using MaterialDesignMessageBox;
using Notifications.Wpf.Core;
using System;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DiaryApp
{
  class SignUpControl : ControlBase
  {
    readonly DbController dbController = new DbController();

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
    public void SignUp()
    {
      var iUser = dbController.GetUserFromDb(UserName).Count == 0;
      if (iUser && CheckPassword())
      {
        CloseAction();
        ShowNotification("Success", "Sign up successfull!", NotificationType.Success);
        CreateNewUser();
      }
      else if (!iUser)
      {
        UserName = string.Empty;
        ShowMessageBox("User already exists!", MessageType.Error, MessageButtons.Ok);
      }
    }

    private void CreateNewUser()
    {
      var newUser = new UserModel()
      {
        LastName = LastName,
        FirstName = FirstName,
        UserName = UserName,
        Password = SecurePasswordHasher.Hash(Helper.ToNormalString(SignInPassword))
      };
      dbController.UserToDb(newUser);
    }

    private bool CheckPassword()
    {
      if (SignInPassword == null || SignInPasswordConfirm == null)
      {
        return false;
      }
      if (Helper.ToNormalString(SignInPassword) != Helper.ToNormalString(SignInPasswordConfirm))
      {
        ShowMessageBox("Passwords are not matching!", MessageType.Error, MessageButtons.Ok);
        SignInPassword = null;
        SignInPasswordConfirm = null;
        return false;
      }
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");
      if (!regex.IsMatch(Helper.ToNormalString(SignInPassword)))
      {
        ShowMessageBox("The entered password does not meet the requirements. Requirements: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit and 1 special character.", MessageType.Error, MessageButtons.Ok);
        SignInPassword = null;
        SignInPasswordConfirm = null;
        return false;
      }
      //return true if every check passes
      return true;
    }
    #endregion
  }
}


