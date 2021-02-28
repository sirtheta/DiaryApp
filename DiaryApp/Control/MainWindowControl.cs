﻿using MaterialDesignMessageBox;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  //Inherit from AbstractPropertyChanged to use the OnPropertyChanged method
  class MainWindowControl : ControlBase
  {
    readonly DbController dbController = new DbController();
    #region Members

    //byte array to hold the Image
    private byte[] imgInByteArr;
    //User ID
    private int loggedInUserId;

    //Definitions for properties
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
    private DateTime _calendarSelectedDate;
    private DiaryEntryModel _datagridSelectedItem;
    private BitmapImage _imageBoxSource;
    private SecureString _signInPassword;
    #endregion

    #region BindingProperties
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
    public string LoggedInUserFullName
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

    public string ChkBxFamilyContent { get => "Family"; }
    public bool FamilyIsChecked
    {
      get => _chkFamilyIsChecked;
      set
      {
        _chkFamilyIsChecked = value;
        OnPropertyChanged();
      }
    }

    public string ChkBxFriendsContent { get => "Friends"; }
    public bool FriendsIsChecked
    {
      get => _chkFriendsIsChecked;
      set
      {
        _chkFriendsIsChecked = value;
        OnPropertyChanged();
      }
    }

    public string ChkBxBirthdayContent { get => "Birthday"; }
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
    public IList CalendarSelectedRange { get; set; }
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

    //**************************************************************************
    //Section for button ICommands
    //**************************************************************************
    #region ICommand
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
    private void ExecuteOpenLoginPopup(object Parameter) => PopupSignInIsOpen = true;
    private void ExecuteCloseLoginPopup(object Parameter) => PopupSignInIsOpen = false;
    private void ExecuteOpenSignUpWindow(object Parameter) => new SignUp().ShowDialog();
    private void ExecuteCloseApplication(object Parameter) => Application.Current.Shutdown();
    private void ExecuteSignOut(object Parameter) => SignOut();
    private void ExecuteSaveEntry(object Parameter) => SaveEntry();
    private void ExecuteAddImage(object Parameter) => AddImage();
    private void ExecuteNew(object Parameter) => ShowAll();
    private void ExecuteDelete(object Parameter) => DeleteSelectedEntry();
    private void ExecuteSearchByTagCommand(object Parameter) => GetEntrysByTag();
    private void ExecuteSearchByDateCommand(object Parameter) => GetEntrysByDate();
    private void ExecuteSearchDatesWithoutEntryCommand(object Parameter) => GetEntrysWithoutDate();
    private void ExecuteShowAll(object Parameter) => ShowAll();
    #endregion

    //Is executed when the window is loaded
    public void OnMainWindowLoad()
    {
      PopupSignInIsOpen = true;
      BtnSignOutVisibility = Visibility.Hidden;
      CalendarSelectedDate = DateTime.Today;
    }

    //**************************************************************************
    //Sign in/sign out section
    //**************************************************************************
    #region SignIn/out

    public void SignIn()
    {
      if (VerifyCredentials())
      {
        MainStackPanelVisibility = true;
        BtnSignOutVisibility = Visibility.Visible;
        BtnSignInVisibility = Visibility.Hidden;
        PopupSignInIsOpen = false;
      }

      SignInUserName = string.Empty;
      SignInPassword = null;
    }

    private bool VerifyCredentials()
    {
      try
      {
        var user = dbController.GetUserName(SignInUserName).Single();

        //verify entered password with the stored password in DB using securePasswordHasher
        if (SecurePasswordHasher.Verify(Helper.ToNormalString(SignInPassword), user.Password))
        {
          loggedInUserId = user.UserId;
          LoggedInUserFullName = dbController.GetFullName(loggedInUserId);
          LoadEntrysFromDb();
          ShowAll();
          Helper.ShowNotification("Success", "Sign in successfull!", NotificationType.Success);
          return true;
        }
        else
        {
          Helper.ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
          return false;
        }
      }
      catch (Exception)
      {
        Helper.ShowMessageBox("No such user! You should sign up now!", MessageType.Error, MessageButtons.Ok);
        return false;
      }
    }

    //Load all entrys from DB with the logged in User
    private void LoadEntrysFromDb()
    {
      _entriesAll = new List<DiaryEntryModel>(dbController.GetEntrysFromDb(loggedInUserId));
    }

    private void SignOut()
    {
      _entriesAll.Clear();
      EntriesToShow.Clear();
      ClearControls();
      loggedInUserId = 0;
      LoggedInUserFullName = string.Empty;
      MainStackPanelVisibility = false;
      BtnSignInVisibility = Visibility.Visible;
      BtnSignOutVisibility = Visibility.Hidden;
    }
    #endregion

    //**************************************************************************
    //Entry Save and delete section
    //**************************************************************************
    #region SaveDelete

    private void SaveEntry()
    {
      if (DatagridSelectedItem == null)
      {
        if (!string.IsNullOrEmpty(EntryText))
        {
          var newEntry = new DiaryEntryModel()
          {
            Text = EntryText,
            Date = CalendarSelectedDate,
            TagFamily = FamilyIsChecked,
            TagFriends = FriendsIsChecked,
            TagBirthday = BirthdayIsChecked,
            ByteImage = imgInByteArr,
            UserId = loggedInUserId
          };

          dbController.EntryToDb(newEntry);
          _entriesAll.Add(newEntry);
          EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.OrderByDescending(d => d.Date));

          Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
          ClearControls();
        }
        else
        {
          Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
        }
      }
      //If a entry is updated, update it in the database
      else if (DatagridSelectedItem is DiaryEntryModel updateEntry)
      {
        var entry = new DiaryEntryModel()
        {
          EntryId = updateEntry.EntryId,
          Text = EntryText,
          Date = CalendarSelectedDate,
          TagFamily = FamilyIsChecked,
          TagFriends = FriendsIsChecked,
          TagBirthday = BirthdayIsChecked,
          ByteImage = imgInByteArr,
          UserId = loggedInUserId
        };

        dbController.EntryToDb(entry);
        _entriesAll.Remove(updateEntry);
        _entriesAll.Add(entry);
        EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.OrderByDescending(d => d.Date));

        Helper.ShowNotification("Success", "Your diary entry successfully updated!", NotificationType.Success);
        ClearControls();
      }
    }

    private void DeleteSelectedEntry()
    {
      if (DatagridSelectedItem != null)
      {
        if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
        {
          //Cast selected Items to Enumerate with foreach
          var entry = DatagridSelectedItems.Cast<DiaryEntryModel>().ToList();
          foreach (var item in entry)
          {
            _entriesAll.Remove(item);
            dbController.DeleteEntryInDb(item);
          }

          Helper.ShowNotification("Success", "Entry Successfull deleted", NotificationType.Success);
          //Update Datagrid
          EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.OrderByDescending(d => d.Date));
          DatagridSelectedItem = null;
          ClearControls();
        }
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }

    private void AddImage()
    {
      //Add Image
      OpenFileDialog dialog = new OpenFileDialog
      {
        Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
      };
      if (dialog.ShowDialog() == true)
      {
        Imager imager = new Imager();
        ImageBoxSource = imager.BitmapForImageSource(dialog.FileName);
        imgInByteArr = imager.ImageToByteArray(dialog.FileName);
      }
    }
    #endregion

    //**************************************************************************
    //Search entry section
    //**************************************************************************
    #region Search
    //Search for entrys by Date
    private void GetEntrysByDate()
    {
      EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.Where(lst => lst.Date.ToString("dd.MM yyyy") == CalendarSelectedDate.ToString("dd.MM yyyy")));
    }

    //Filter all entrys by clicked tag. 
    //If more than one tag is selected, the range from the second or third will be added to the existing list
    //To remove duplicate, use Distinct at the end
    private void GetEntrysByTag()
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

      EntriesToShow = new ObservableCollection<DiaryEntryModel>(query.Distinct().OrderByDescending(d => d.Date));
    }

    //Search for dates withtout entrys. Display all found dates in the datagrid.
    //By selecting one of the dates, you can add a new entry on that specific date
    private void GetEntrysWithoutDate()
    {

      if (CalendarSelectedRange.Count > 1)
      {
        var onlyDateList = new List<DiaryEntryModel>();
        //Cast selected range to a DateTime list to Enumerate with foreach
        var calendarSelectedDateRange = CalendarSelectedRange.Cast<DateTime>().ToList();
        foreach (var entry in _entriesAll)
        {
          calendarSelectedDateRange.Remove(entry.Date.Date);
        }
        foreach (var item in calendarSelectedDateRange)
        {
          var onlyDate = new DiaryEntryModel()
          {
            Date = item
          };
          onlyDateList.Add(onlyDate);
        }
        EntriesToShow = new ObservableCollection<DiaryEntryModel>(onlyDateList.OrderByDescending(d => d.Date));
        ClearControls();
      }
      else
      {
        Helper.ShowMessageBox("Please specify range to search for in the calendar.", MessageType.Warning, MessageButtons.Ok);
      }
    }
    #endregion

    //If item is selected in Datagrid, show it
    private void ShowSelectedItem()
    {
      if (DatagridSelectedItem != null)
      {
        Imager imager = new Imager();
        var selected = DatagridSelectedItem;

        EntryText = selected.Text;
        FamilyIsChecked = selected.TagFamily;
        FriendsIsChecked = selected.TagFriends;
        BirthdayIsChecked = selected.TagBirthday;
        CalendarSelectedDate = selected.Date;
        imgInByteArr = selected.ByteImage;
        ImageBoxSource = imager.ImageFromByteArray(imgInByteArr);
      }
    }

    private void ShowAll()
    {
      ClearControls();
      EntriesToShow = new ObservableCollection<DiaryEntryModel>(_entriesAll.OrderByDescending(d => d.Date));
    }

    private void ClearControls()
    {
      EntryText = string.Empty;
      FamilyIsChecked = false;
      FriendsIsChecked = false;
      BirthdayIsChecked = false;
      ImageBoxSource = null;
      DatagridSelectedItem = null;
      imgInByteArr = null;
    }
  }
}
