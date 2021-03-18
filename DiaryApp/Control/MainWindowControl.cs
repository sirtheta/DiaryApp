using MaterialDesignMessageBoxSirTheta;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

//set all internals visible for the unittests
[assembly: InternalsVisibleTo("DiaryApp.Test", AllInternalsVisible = true)]

/// <summary>
/// Main Control Class for the MainWindow view
/// </summary>
namespace DiaryApp
{
  //Inherit from AbstractPropertyChanged to use the OnPropertyChanged method
  internal class MainWindowControl : ControlBase
  {

    #region Members
    private byte[] _imgInByteArr;
    private int _signedInUserId;
    public int SignedInUserId
    {
      get => _signedInUserId;
      set { _signedInUserId = value; }
    }

    private string _loggedInUserFullName;
    private string _entryText;
    private string _signInUserName;
    private bool _chkFamilyIsChecked;
    private bool _chkFriendsIsChecked;
    private bool _chkBirthdayIsChecked;
    private bool _popupSignInIsOpen;
    private bool _mainStackPanelVisibility;
    private Visibility _btnLoginVisibility;
    private Visibility _btnSignOutVisibility;
    private List<DiaryEntryModel> _entriesAll;
    private ObservableCollection<DiaryEntryModel> _entriesToShow;
    private IList _calendarSelectedRange;
    private DateTime _calendarSelectedDate;
    private DiaryEntryModel _datagridSelectedItem;
    private BitmapImage _imageBoxSource;
    private SecureString _signInPassword;
    #endregion

    #region BindingProperties
    //all properties with binding to MainWindow.xaml
    public ObservableCollection<DiaryEntryModel> EntriesToShow
    {
      get => _entriesToShow;
      private set
      {
        _entriesToShow = value;
        OnPropertyChanged();
      }
    }
    //property to get the full name of the user to in MainWindow
    public string SignedInUserFullName
    {
      get => _loggedInUserFullName;
      set
      {
        _loggedInUserFullName = value;
        OnPropertyChanged();
      }
    }

    public string EntryText
    {
      get => _entryText;
      set
      {
        _entryText = value;
        OnPropertyChanged();
      }
    }

    public string ChkBxFamilyContent
    {
      get => "Family";
    }
    public bool FamilyIsChecked
    {
      get => _chkFamilyIsChecked;
      set
      {
        _chkFamilyIsChecked = value;
        OnPropertyChanged();
      }
    }

    public string ChkBxFriendsContent
    {
      get => "Friends";
    }
    public bool FriendsIsChecked
    {
      get => _chkFriendsIsChecked;
      set
      {
        _chkFriendsIsChecked = value;
        OnPropertyChanged();
      }
    }

    public string ChkBxBirthdayContent
    {
      get => "Birthday";
    }
    public bool BirthdayIsChecked
    {
      get => _chkBirthdayIsChecked;
      set
      {
        _chkBirthdayIsChecked = value;
        OnPropertyChanged();
      }
    }

    public bool MainStackPanelVisibility
    {
      get => _mainStackPanelVisibility;
      set
      {
        _mainStackPanelVisibility = value;
        OnPropertyChanged();
      }
    }

    public DateTime CalendarSelectedDate
    {
      get => _calendarSelectedDate;
      set
      {
        _calendarSelectedDate = value;
        OnPropertyChanged();
      }
    }

    public IList CalendarSelectedRange
    {
      get => _calendarSelectedRange;
      set
      {
        _calendarSelectedRange = value;
      }
    }

    public IList DatagridSelectedItems { get; set; }

    public DiaryEntryModel DatagridSelectedItem
    {
      get => _datagridSelectedItem;
      set
      {
        _datagridSelectedItem = value;
        OnPropertyChanged();
        ShowSelectedItem();
      }
    }

    public BitmapImage ImageBoxSource
    {
      get => _imageBoxSource;
      set
      {
        _imageBoxSource = value;
        OnPropertyChanged();
      }
    }

    //SignIn Binding
    public string SignInUserName
    {
      get => _signInUserName;
      set
      {
        _signInUserName = value;
        OnPropertyChanged();
      }
    }
    public bool PopupSignInIsOpen
    {
      get => _popupSignInIsOpen;
      set
      {
        _popupSignInIsOpen = value;
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
    public Visibility BtnSignInVisibility
    {
      get => _btnLoginVisibility;
      set
      {
        _btnLoginVisibility = value;
        OnPropertyChanged();
      }
    }
    public Visibility BtnSignOutVisibility
    {
      get => _btnSignOutVisibility;
      set
      {
        _btnSignOutVisibility = value;
        OnPropertyChanged();
      }
    }
    #endregion

    #region ICommands
    //contains all commands with binding from MainView.xaml
    public ICommand OpenSignInPopupCommand
    {
      get => new RelayCommand<object>(ExecuteOpenLoginPopup);
    }

    public ICommand CloseSignInPopupCommand
    {
      get => new RelayCommand<object>(ExecuteCloseLoginPopup);
    }

    public ICommand OpenSignUpWindowCommand
    {
      get => new RelayCommand<object>(ExecuteOpenSignUpWindow);
    }

    public ICommand CloseApplicationCommand
    {
      get => new RelayCommand<object>(ExecuteCloseApplication);
    }

    public ICommand SignOutCommand
    {
      get => new RelayCommand<object>(ExecuteSignOut);
    }

    public ICommand SaveEntryCommand
    {
      get => new RelayCommand<object>(ExecuteSaveEntry);
    }

    public ICommand AddImageCommand
    {
      get => new RelayCommand<object>(ExecuteAddImage);
    }

    public ICommand NewCommand
    {
      get => new RelayCommand<object>(ExecuteNew);
    }

    public ICommand DeleteCommand
    {
      get => new RelayCommand<object>(ExecuteDelete);
    }

    public ICommand SearchByTagCommand
    {
      get => new RelayCommand<object>(ExecuteSearchByTagCommand);
    }

    public ICommand SearchByDateCommand
    {
      get => new RelayCommand<object>(ExecuteSearchByDateCommand);
    }

    public ICommand SearchDatesWithoutEntryCommand
    {
      get => new RelayCommand<object>(ExecuteSearchDatesWithoutEntryCommand);
    }

    public ICommand ShowAllCommand
    {
      get => new RelayCommand<object>(ExecuteShowAll);
    }
    #endregion

    #region ExecuteCommands
    //Executen methods of the calling ICommands
    private void ExecuteOpenLoginPopup(object Parameter) => PopupSignInIsOpen = true;
    private void ExecuteCloseLoginPopup(object Parameter) => PopupSignInIsOpen = false;
    private void ExecuteOpenSignUpWindow(object Parameter) => new SignUp().ShowDialog();
    private void ExecuteCloseApplication(object Parameter) => Application.Current.Shutdown();
    private void ExecuteSignOut(object Parameter) => SignOut();
    private void ExecuteSaveEntry(object Parameter) => SaveOrUpdateEntry();
    private void ExecuteAddImage(object Parameter) => AddImage();
    private void ExecuteNew(object Parameter) => ClearAndShow();
    private void ExecuteDelete(object Parameter) => AskToDeleteSelectedEntry();
    private void ExecuteSearchByTagCommand(object Parameter) => ShowEntrysByTag();
    private void ExecuteSearchByDateCommand(object Parameter) => GetEntrysByDate();
    private void ExecuteSearchDatesWithoutEntryCommand(object Parameter) => ShowDatesWithoutEntry();
    private void ExecuteShowAll(object Parameter) => ClearAndShow();
    #endregion

    //Is executed when the window is loaded
    public void OnMainWindowLoad()
    {
      PopupSignInIsOpen = true;
      BtnSignOutVisibility = Visibility.Hidden;
      CalendarSelectedDate = DateTime.Today;
    }

    ///**************************************************************************
    ///Methods
    ///**************************************************************************
    #region SignIn/out
    //called directly from MainWindow.xaml
    public void SignIn()
    {
      if (VerifyUser())
      {
        ShowNotification("Success", "Sign in successfull!", NotificationType.Success);
      }
      else
      {
        ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
      }
      SignInUserName = string.Empty;
      SignInPassword = null;
    }

    //verify entered password with the stored password in DB using securePasswordHasher
    internal bool VerifyUser()
    {
      var user = DbController.GetUserFromDb(SignInUserName).SingleOrDefault();

      if (SignInPassword != null && SecurePasswordHasher.Verify(ToNormalString(SignInPassword), user.Password))
      {
        Verified(user);
        return true;
      }
      return false;
    }

    //this is called when the user is successfull verified
    internal void Verified(UserModel user)
    {
      SignedInUserId = user.UserId;
      MainStackPanelVisibility = true;
      BtnSignOutVisibility = Visibility.Visible;
      BtnSignInVisibility = Visibility.Hidden;
      PopupSignInIsOpen = false;
      SignedInUserFullName = DbController.GetFullName(SignedInUserId);
      LoadEntrysFromDb();
      ClearAndShow();
    }

    //Load all entrys from DB with the logged in User
    internal void LoadEntrysFromDb()
    {
      _entriesAll = new List<DiaryEntryModel>(DbController.GetEntrysFromDb(SignedInUserId));
    }

    //clear all elements on signOut command
    private void SignOut()
    {
      ClearControls();
      _entriesAll.Clear();
      EntriesToShow.Clear();
      SignedInUserId = 0;
      SignedInUserFullName = string.Empty;
      MainStackPanelVisibility = false;
      BtnSignInVisibility = Visibility.Visible;
      BtnSignOutVisibility = Visibility.Hidden;
    }
    #endregion

    #region SaveDelete

    private void SaveOrUpdateEntry()
    {
      //determine wehter the entry should be updated or created
      //if datacrid selection is null, a new entry is created
      if (DatagridSelectedItem == null)
      {
        if (!string.IsNullOrEmpty(EntryText))
        {
          SaveEntry();
          ClearAndShow();
          ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
        }
        else
        {
          ShowMessageBox("No text to save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
        }
      }
      //If a entry is updated, update it in the database
      else if (DatagridSelectedItem is DiaryEntryModel updateEntry)
      {
        UpdateEntry(updateEntry);
        ClearAndShow();
        ShowNotification("Success", "Your diary entry is updated!", NotificationType.Success);
      }
    }

    internal void SaveEntry()
    {
      var newEntry = new DiaryEntryModel()
      {
        Text = EntryText,
        Date = CalendarSelectedDate,
        TagFamily = FamilyIsChecked,
        TagFriends = FriendsIsChecked,
        TagBirthday = BirthdayIsChecked,
        ByteImage = _imgInByteArr,
        UserId = SignedInUserId
      };

      DbController.EntryToDb(newEntry);
      _entriesAll.Add(newEntry);
    }

    internal void UpdateEntry(DiaryEntryModel updateEntry)
    {
      var entry = new DiaryEntryModel()
      {
        EntryId = updateEntry.EntryId,
        Text = EntryText,
        Date = CalendarSelectedDate,
        TagFamily = FamilyIsChecked,
        TagFriends = FriendsIsChecked,
        TagBirthday = BirthdayIsChecked,
        ByteImage = _imgInByteArr,
        UserId = SignedInUserId
      };

      DbController.EntryToDb(entry);
      _entriesAll.Remove(updateEntry);
      _entriesAll.Add(entry);
    }

    private void AskToDeleteSelectedEntry()
    {
      if (DatagridSelectedItem != null && DatagridSelectedItem.EntryId != 0)
      {
        if (ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
        {
          //Cast selected Items to Enumerate with foreach
          DeleteEntry(DatagridSelectedItems.Cast<DiaryEntryModel>().ToList());
          ShowNotification("Success", "Entry Successfull deleted", NotificationType.Success);
          //Update Datagrid
          ClearAndShow();
        }
      }
      else
      {
        ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }

    internal void DeleteEntry(List<DiaryEntryModel> entry)
    {
      //Remove every selected entry in the list and in the DB
      foreach (var item in entry)
      {
        _entriesAll.Remove(item);
        DbController.DeleteEntryInDb(item);
      }
    }

    //is called from the addImage button
    private void AddImage()
    {
      var fileUri = LoadImage();
      if (fileUri != null)
      {
        ProcessImage(fileUri);
        DisplayImage();
      }
    }
    #endregion

    #region Search
    ///Search for entrys by Date 
    ///compareing is made with string due to the time element in the DateTime struct
    private void GetEntrysByDate()
    {
      EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.Where(lst => lst.Date == CalendarSelectedDate));
    }

    private void ShowEntrysByTag()
    {
      Show(GetEntrysByTag());
    }

    ///Filter all entries by clicked tag. If more than one tag is selected, 
    ///the range from the second or third will be added to the existing 
    ///list to remove duplicate, use Distinct() at the end
    internal List<DiaryEntryModel> GetEntrysByTag()
    {
      var query = new List<DiaryEntryModel>();
      if (FamilyIsChecked)
      {
        query = _entriesAll.Where(lst => lst.TagFamily).ToList();
      }

      if (FriendsIsChecked)
      {
        if (query.Count != 0)
        {
          query.AddRange(_entriesAll.Where(lst => lst.TagFriends).ToList());
        }
        else
        {
          query = _entriesAll.Where(lst => lst.TagFriends).ToList();
        }
      }

      if (BirthdayIsChecked)
      {
        if (query.Count != 0)
        {
          query.AddRange(_entriesAll.Where(lst => lst.TagBirthday).ToList());
        }
        else
        {
          query = _entriesAll.Where(lst => lst.TagBirthday).ToList();
        }
      }

      if (!BirthdayIsChecked && !FamilyIsChecked && !FriendsIsChecked)
      {
        query = _entriesAll.Where(lst => !lst.TagBirthday && !lst.TagFriends && !lst.TagFamily).ToList();
      }

      return query.Distinct().ToList();
    }

    ///Search for dates withtout entrys. Display all found dates in the datagrid.
    ///By selecting one of the dates, you can add a new entry on that specific date
    internal void ShowDatesWithoutEntry()
    {
      if (!GetDatesWithoutEntry())
      {
        ShowMessageBox("Please specify range to search for in the calendar.", MessageType.Warning, MessageButtons.Ok);
      }
    }

    internal bool GetDatesWithoutEntry()
    {
      if (CalendarSelectedRange.Count <= 1)
      {
        return false;
      }

      var onlyDateList = new List<DiaryEntryModel>();
      //Cast selected range to a DateTime list to Enumerate with foreach
      var calendarSelectedDateRange = CalendarSelectedRange.Cast<DateTime>().ToList();

      //remove all dates from the selected date list where a entry with this date exist
      foreach (var entry in _entriesAll)
      {
        calendarSelectedDateRange.Remove(entry.Date.Date);
      }
      //add every leftover date to the list to show in the datagrid
      foreach (var item in calendarSelectedDateRange)
      {
        var onlyDate = new DiaryEntryModel()
        {
          Date = item
        };
        onlyDateList.Add(onlyDate);
      }

      Show(onlyDateList);
      return true;
    }
    #endregion

    ///If item is selected in Datagrid, show it
    ///TODO: Check if input has changed. as of now, not saved changes ar lost
    private void ShowSelectedItem()
    {
      if (DatagridSelectedItem != null)
      {
        EntryText = DatagridSelectedItem.Text;
        FamilyIsChecked = DatagridSelectedItem.TagFamily;
        FriendsIsChecked = DatagridSelectedItem.TagFriends;
        BirthdayIsChecked = DatagridSelectedItem.TagBirthday;
        CalendarSelectedDate = DatagridSelectedItem.Date;
        _imgInByteArr = DatagridSelectedItem.ByteImage;
        DisplayImage();
      }
    }

    //Clear all controls and display all entries in datagrid
    private void ClearAndShow()
    {
      ClearControls();
      EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.OrderByDescending(d => d.Date));
    }
    //Displays the input list in the datagrid
    private void Show(List<DiaryEntryModel> entry)
    {
      EntriesToShow = new ObservableCollection<DiaryEntryModel>(entry.OrderByDescending(d => d.Date));
    }

    private void ClearControls()
    {
      EntryText = string.Empty;
      FamilyIsChecked = false;
      FriendsIsChecked = false;
      BirthdayIsChecked = false;
      ImageBoxSource = null;
      DatagridSelectedItem = null;
      _imgInByteArr = null;
    }

    //Methods for Image (load and show)
    #region Image
    //Load the image uri from file with file dialog
    private string LoadImage()
    {
      string retVal = null;
      //Load image with dialog
      OpenFileDialog dialog = new OpenFileDialog
      {
        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
      };
      if (dialog.ShowDialog() == true)
      {
        retVal = dialog.FileName;
      }
      return retVal;
    }

    ///call the imageconverter, standard max size is 1024 
    ///specify if needed
    private void ProcessImage(string fileUri)
    {
      _imgInByteArr = Imager.ImageToByteArray(fileUri);
    }

    //Display image in GUI-->ImageBoxSource
    private void DisplayImage()
    {
      ImageBoxSource = Imager.ImageFromByteArray(_imgInByteArr);
    }
    #endregion
  }
}
