using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  class Control : AbstractPropertyChanged
  {
    readonly Model model = new Model();
    private IList<DiaryEntryDb> _EntriesAll = new List<DiaryEntryDb>();
    private ObservableCollection<DiaryEntryDb> _EntriesToShow = new ObservableCollection<DiaryEntryDb>();
    public ObservableCollection<DiaryEntryDb> EntriesToShow
    {
      get => _EntriesToShow;
      private set
      {
        _EntriesToShow = value;
        OnPropertyChanged();
      }
    }

    //byte array to hold the Image
    private byte[] imgInByteArr;
    //User ID
    private int loggedInUserId;

    private string _loggedInUserFullName;
    private string _entryText;
    private bool _chkFamilyIsChecked;
    private bool _chkFriendsIsChecked;
    private bool _chkBirthdayIsChecked;
    private DateTime _calendarSelectedDate;
    private DiaryEntryDb _datagridSelectedItem;
    private BitmapImage _imageBoxSource;
    private string _signInUserName;
    private SecureString _signInPassword;

    #region Properties
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

    public DateTime CalendarSelectedDate
    {
      get => _calendarSelectedDate;
      set
      {
        _calendarSelectedDate = value;
        OnPropertyChanged();
      }
    }
    public DiaryEntryDb DatagridSelectedItem
    {
      get => _datagridSelectedItem;
      set
      {
        _datagridSelectedItem = value;
        OnPropertyChanged();
      }
    }
    public IList SelectedItems { get; set; }
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
    public SecureString SignInPassword
    {
      get => _signInPassword;
      set
      {
        _signInPassword = value;
        OnPropertyChanged();
      }
    }
    #endregion

    //**************************************************************************
    //Sign in/sign out section
    //**************************************************************************
    #region SignIn/out

    public bool VerifyCredentials()
    {
      try
      {
        var user = model.GetUser(SignInUserName).Single();

        if (SecurePasswordHasher.Verify(Helper.ToNormalString(SignInPassword), user.Password))
        {
          loggedInUserId = user.UserId;
          LoggedInUserFullName = model.FullName(loggedInUserId);
          LoadEntrysFromDb();
          ShowAll();
          Helper.ShowNotification("Success", "Sign in successfull!", NotificationType.Success);
          SignInUserName = string.Empty;
          SignInPassword = null;
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
        Helper.ShowMessageBox("No such user! Sign up now!", MessageType.Error, MessageButtons.Ok);
        SignInUserName = string.Empty;
        SignInPassword = null;
        return false;
      }
    }

    //Load all entrys from DB with the logged in User
    public void LoadEntrysFromDb()
    {
      _EntriesAll = new List<DiaryEntryDb>(model.GetEntrysFromDb(loggedInUserId));
    }

    public void SignOut()
    {
      _EntriesAll.Clear();
      EntriesToShow.Clear();
      ClearControls();
      loggedInUserId = 0;
      LoggedInUserFullName = string.Empty;
    }
    #endregion

    //**************************************************************************
    //Entry Save and delete section
    //**************************************************************************
    #region SaveDelete
    //If item is selected in Datagrid, show it
    public void ShowSelectedItem()
    {
      //before displaying selected item, clear entire input area
      EntryText = string.Empty;
      FamilyIsChecked = false;
      FriendsIsChecked = false;
      BirthdayIsChecked = false;
      CalendarSelectedDate = DateTime.Now;
      ImageBoxSource = null;

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

    public void SaveEntry()
    {
      if (DatagridSelectedItem == null)
      {
        if (!string.IsNullOrEmpty(EntryText))
        {
          var newEntry = new DiaryEntryDb()
          {
            Text = EntryText,
            Date = CalendarSelectedDate,
            TagFamily = FamilyIsChecked,
            TagFriends = FriendsIsChecked,
            TagBirthday = BirthdayIsChecked,
            ByteImage = imgInByteArr,
            UserId = loggedInUserId
          };

          model.EntryToDb(newEntry);
          _EntriesAll.Add(newEntry);
          EntriesToShow = new ObservableCollection<DiaryEntryDb>(_EntriesAll.OrderByDescending(d => d.Date));

          Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
          ClearControls();
        }
        else
        {
          Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
        }
      }
      //If a entry is updated, update it in the database
      else if (DatagridSelectedItem is DiaryEntryDb updateEntry)
      {
        var entry = new DiaryEntryDb()
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

        model.EntryToDb(entry);
        _EntriesAll.Remove(updateEntry);
        _EntriesAll.Add(entry);
        EntriesToShow = new ObservableCollection<DiaryEntryDb>(_EntriesAll.OrderByDescending(d => d.Date));

        Helper.ShowNotification("Success", "Your diary entry successfully updated!", NotificationType.Success);
      }
    }

    public void DeleteSelectedEntry()
    {
      if (DatagridSelectedItem != null)
      {
        if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
        {
          //Cast selected Items to Enumerate with foreach
          var entry = SelectedItems.Cast<DiaryEntryDb>().ToList();
          foreach (var item in entry)
          {
            _EntriesAll.Remove(item);
            model.DeleteEntryInDb(item);
          }

          Helper.ShowNotification("Success", "Entry Successfull deleted", NotificationType.Success);
          //Update Datagrid
          EntriesToShow = new ObservableCollection<DiaryEntryDb>(_EntriesAll.OrderByDescending(d => d.Date));
          DatagridSelectedItem = null;
          ClearControls();
        }
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }

    public void AddImage()
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
    public void GetEntrysByDate()
    {
      EntriesToShow = new ObservableCollection<DiaryEntryDb>(_EntriesAll.Where(lst => lst.Date.ToString("dd.MMMM yyyy") == CalendarSelectedDate.ToString("dd.MMMM yyyy")));
    }

    //Filter all entrys by clicked tag. 
    //If more than one tag is selected, the range from the second or third will be added to the existing list
    public void GetEntrysByTag()
    {
      var query = _EntriesAll.ToList();
      if (FamilyIsChecked)
      {
        query = _EntriesAll.Where(lst => lst.TagFamily).ToList();
      }

      if (FriendsIsChecked)
      {
        if (query != _EntriesAll.ToList())
        {
          query.AddRange(_EntriesAll.Where(lst => lst.TagFriends).ToList());
        }
        else
        {
          query = _EntriesAll.Where(lst => lst.TagFriends).ToList();
        }
      }

      if (BirthdayIsChecked)
      {
        if (query != _EntriesAll.ToList())
        {
          query.AddRange(_EntriesAll.Where(lst => lst.TagBirthday).ToList());
        }
        else
        {
          query = _EntriesAll.Where(lst => lst.TagBirthday).ToList();
        }
      }
      EntriesToShow = new ObservableCollection<DiaryEntryDb>(query.Distinct());
    }
    #endregion

    public void ShowAll()
    {
      ClearControls();
      EntriesToShow = new ObservableCollection<DiaryEntryDb>(_EntriesAll);
    }

    public void ClearControls()
    {
      EntryText = string.Empty;
      FamilyIsChecked = false;
      FriendsIsChecked = false;
      BirthdayIsChecked = false;
      CalendarSelectedDate = DateTime.Now;
      ImageBoxSource = null;
      DatagridSelectedItem = null;
      imgInByteArr = null;
    }
  }
}
