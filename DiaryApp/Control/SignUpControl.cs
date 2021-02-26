using Notifications.Wpf.Core;
using System.Security;
using System.Text.RegularExpressions;

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

    //**************************************************************************
    //Methods
    //**************************************************************************
    #region Methods
    public bool SignUp()
    {
      var iUser = dbController.GetUserName(UserName).Count == 0;
      if (iUser && CheckPassword())
      {
        CreateNewUser();
        Helper.ShowNotification("Success", "Sign up successfull!", NotificationType.Success);
        return true;
      }
      else if (!iUser)
      {
        Helper.ShowMessageBox("User already exists!", MessageType.Error, MessageButtons.Ok);
        UserName = string.Empty;
        return false;
      }
      else return false;
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
      Regex regex = new Regex(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$");
      if (SignInPassword == null || SignInPasswordConfirm == null)
      {
        return false;
      }
      if (Helper.ToNormalString(SignInPassword) != Helper.ToNormalString(SignInPasswordConfirm))
      {
        Helper.ShowMessageBox("Passwords are not matching!", MessageType.Error, MessageButtons.Ok);
        SignInPassword = null;
        SignInPasswordConfirm = null;
        return false;
      }
      if (!regex.IsMatch(Helper.ToNormalString(SignInPassword)))
      {
        Helper.ShowMessageBox("The entered password does not meet the requirements. Requirements: minimum 8 characters, 1 lowercase, 1 uppercase, 1 digit and 1 special character.", MessageType.Error, MessageButtons.Ok);
        SignInPassword = null;
        SignInPasswordConfirm = null;
        return false;
      }
      return true;
    }
    #endregion
  }
}


