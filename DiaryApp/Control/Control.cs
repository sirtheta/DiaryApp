using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  public class Control
  {
    readonly Model model = new Model();
    List<DiaryEntryDb> lstEntry;

    TextBox entryText;
    CheckBox chkBxFamily;
    CheckBox chkBxFriends;
    CheckBox chkBxBirthday;
    Calendar calendar;
    DataGrid dgManageEntrys;
    Image imageBox;

    //empty constructor needed to initialize MainWindow
    public Control() { }

    public Control(TextBox t_entryText,
                   CheckBox t_chkBxFamily,
                   CheckBox t_chkBxFriends,
                   CheckBox t_chkBxBirthday,
                   Calendar t_calendar,
                   DataGrid t_dgManageEntrys,
                   Image t_imageBox)
    {
      entryText = t_entryText;
      chkBxFamily = t_chkBxFamily;
      chkBxFriends = t_chkBxFriends;
      chkBxBirthday = t_chkBxBirthday;
      calendar = t_calendar;
      dgManageEntrys = t_dgManageEntrys;
      imageBox = t_imageBox;
    }

    public string LoggedInUserFullName
    {
      get
      {
        if (LooggedInUserId != 0)
        {
          return model.FullName(LooggedInUserId);
        }
        return null;
      }
    }
    public string ChkBxFamily { get { return "Family"; } }
    public string ChkBxFriends { get { return "Friends"; } }
    public string ChkBxBirthday { get { return "Birthday"; } }
    public int LooggedInUserId { get; set; }

    private string SelectedFileName { get; set; }
    private byte[] ImgInByteArr { get; set; }

    //Load all entrys from DB with the logged in User
    public void LoadEntrysFromDb()
    {
      lstEntry = new List<DiaryEntryDb>(model.GetEntrysFromDb(LooggedInUserId));
      //Load lstEntry to Datagrid
      dgManageEntrys.ItemsSource = lstEntry;
    }

    //**************************************************************************
    //Sign in/sign out section
    //**************************************************************************
    #region SignIn/out

    public bool VerifyCredentials(TextBox userName, PasswordBox password)
    {
      try
      {
        var user = model.GetUser(userName.Text).Single();

        if (SecurePasswordHasher.Verify(password.Password, user.Password))
        {
          LooggedInUserId = user.UserId;
          LoadEntrysFromDb();
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
        userName.Text = "";
        password.Password = "";
        return false;
      }
    }
    public void SignOut()
    {
      lstEntry.Clear();
      dgManageEntrys.Items.Refresh();
      LooggedInUserId = 0;
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
      ClearControls();
      if (dgManageEntrys.SelectedItem != null)
      {
        var selected = dgManageEntrys.SelectedItem as DiaryEntryDb;

        entryText.Text = selected.Text;
        chkBxFamily.IsChecked = selected.TagFamily;
        chkBxFriends.IsChecked = selected.TagFriends;
        chkBxBirthday.IsChecked = selected.TagBirthday;
        calendar.SelectedDate = selected.Date;
        ImageFromByteArray(selected.ByteImage);
      }
    }

    public void SaveEntry()
    {
      if (dgManageEntrys.SelectedItem == null)
      {
        if (entryText.Text != "")
        {
          var newEntry = new DiaryEntryDb()
          {
            Text = entryText.Text,
            Date = calendar.SelectedDate.Value,
            TagFamily = (bool)chkBxFamily.IsChecked,
            TagFriends = (bool)chkBxFriends.IsChecked,
            TagBirthday = (bool)chkBxBirthday.IsChecked,
            ByteImage = ImgInByteArr,
            UserId = LooggedInUserId
          };

          model.EntryToDb(newEntry);
          lstEntry.Add(newEntry);
          dgManageEntrys.ItemsSource = lstEntry.OrderByDescending(d => d.Date).ToList();

          Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
        }
        else
        {
          Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
        }
      }
      //If a entry is updated, update it in the database
      else if (dgManageEntrys.SelectedItem is DiaryEntryDb updateEntry)
      {
        var entry = new DiaryEntryDb()
        {
          EntryId = updateEntry.EntryId,
          Text = entryText.Text,
          Date = calendar.SelectedDate.Value,
          TagFamily = (bool)chkBxFamily.IsChecked,
          TagFriends = (bool)chkBxFriends.IsChecked,
          TagBirthday = (bool)chkBxBirthday.IsChecked,
          ByteImage = ImgInByteArr,
          UserId = updateEntry.UserId
        };

        model.EntryToDb(entry);
        //remove old entry from list and then add the updated one and order it by date
        lstEntry.Remove(updateEntry);
        lstEntry.Add(entry);
        dgManageEntrys.ItemsSource = lstEntry.OrderByDescending(d => d.Date).ToList();

        Helper.ShowNotification("Success", "Your diary entry successfully updated!", NotificationType.Success);
      }
      //Update Datagrid
      dgManageEntrys.Items.Refresh();
    }

    public void DeleteSelectedEntry()
    {
      if (dgManageEntrys.SelectedItem != null)
      {
        if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
        {
          //Cast selected Items to Enumerate with foreach
          var entry = dgManageEntrys.SelectedItems.Cast<DiaryEntryDb>().ToList();
          foreach (var item in entry)
          {
            lstEntry.Remove(item);
            model.DeleteEntryInDb(item);
          }

          Helper.ShowNotification("Success", "Entry Successfull deleted", NotificationType.Success);
          //Update Datagrid
          dgManageEntrys.ItemsSource = lstEntry.OrderByDescending(d => d.Date).ToList();
          dgManageEntrys.SelectedItem = null;
          ClearControls();
        }
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
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
      //Convert date to string because I cant ignore the Timepart
      dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.Date.ToString("dd.MMMM yyyy") == calendar.SelectedDate.Value.ToString("dd.MMMM yyyy")).ToList();
    }

    public void ShowAll(DataGrid dgManageEntrys)
    {
      ClearControls();
      dgManageEntrys.SelectedItem = null;
      dgManageEntrys.ItemsSource = lstEntry;
    }

    public void ClearControls()
    {
      entryText.Text = "";
      chkBxFamily.IsChecked = false;
      chkBxFriends.IsChecked = false;
      chkBxBirthday.IsChecked = false;
      calendar.SelectedDate = DateTime.Now;
      imageBox.Source = null;
    }

    public void GetEntrysByTag()
    {
      bool bFamily = (bool)chkBxFamily.IsChecked;
      bool bFriends = (bool)chkBxFriends.IsChecked;
      bool bBirthday = (bool)chkBxBirthday.IsChecked;

      if (bFamily == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFamily).ToList();
      }
      else if (bFriends == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFriends! & lst.TagBirthday! & lst.TagFamily).ToList();
      }
      else if (bBirthday == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagBirthday).ToList();
      }
      else
      {
        dgManageEntrys.ItemsSource = lstEntry;
      }
    }
    #endregion

    public void AddImage()
    {
      //Add Image
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
      if (dialog.ShowDialog() == true)
      {
        SelectedFileName = dialog.FileName;
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(SelectedFileName);
        bitmap.EndInit();
        imageBox.Source = bitmap;
        ImageToByteArray();
      }
    }


    private void ImageToByteArray()
    {
      if (SelectedFileName != null)
      {
        ImgInByteArr = File.ReadAllBytes(SelectedFileName);
      }
    }

    private async void ImageFromByteArray(byte[] array)
    {
      if (array != null)
      {
        var image = new BitmapImage();
        await using (var ms = new MemoryStream(array))
        {
          image.BeginInit();
          image.CacheOption = BitmapCacheOption.OnLoad;
          image.StreamSource = ms;
          image.EndInit();
        }
        imageBox.Source = image;
      }
    }
  }
}
