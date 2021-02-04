using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  public class Control
  {
    readonly Model model = new Model();

    ObservableCollection<DiaryEntryDb> lstEntry = new ObservableCollection<DiaryEntryDb>();
    string selectedFileName;

    public string LoggedInUser { get { return model.FullName(); } }
    public string ChkBxFamily { get { return "Family"; } }
    public string ChkBxFriends { get { return "Friends"; } }
    public string ChkBxBirthday { get { return "Birthday"; } }
    public static int UserId { get; set; }

    public void LoadEntrysFromDb(DataGrid dgManageEntrys)
    {
      foreach (var item in Model.GetEntrysFromDb(UserId))
      {
        lstEntry.Add(item);
      } 
      dgManageEntrys.ItemsSource = lstEntry;
    }


    #region Login
    private bool CheckForValidUser(string userName, string password)
    {
      using (var db = new DiaryContext())
      {
        if (db.Users.Any(o => o.UserName == userName) && db.Users.Any(o => o.Password == password))
        {
          //Set userID to use in MainWindowLogic
          UserId = model.GetUserId(userName);
          return true;
        }
      }
      return false;
    }

    public bool Login(TextBox userName, PasswordBox password, Control control)
    {

      if (CheckForValidUser(userName.Text, password.Password))
      {
        Window main = new MainWindow(control);
        main.Show();
        return true;
      }
      else
      {
        Helper.ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
        return false;
      }
    } 
    #endregion


    #region MainWindow
    #region SaveDelete
    public void SaveEntry(TextBox entryText, CheckBox chkBxFamily, CheckBox chkBxFriends, CheckBox chkBxBirthday, Calendar date, DataGrid dgManageEntrys)
    {
      if (entryText.Text != "")
      {
        var newEntry = new DiaryEntryDb()
        {
          Text = entryText.Text,
          Date = date.SelectedDate.Value.ToString("dd. MMMM yyyy"),
          TagFamily = (bool)chkBxFamily.IsChecked,
          TagFriends = (bool)chkBxFriends.IsChecked,
          TagBirthday = (bool)chkBxBirthday.IsChecked,
          ByteImage = ImageToByteArray(),
          UserId = UserId
        };
        model.EntryToDb(newEntry);
        lstEntry.Add(newEntry);

        Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);

        //Update Datagrid
        dgManageEntrys.Items.Refresh();

        //clear text in input field and checkBoxes
        entryText.Text = "";
        chkBxFamily.IsChecked = false;
        chkBxFriends.IsChecked = false;
        chkBxBirthday.IsChecked = false;
      }
      else
      {
        Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
      }
    }
    //public void DeleteSelectedEntry(DataGrid dgManageEntrys)
    public void DeleteSelectedEntry(DataGrid dgManageEntrys)
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
          dgManageEntrys.Items.Refresh();
        }
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }
    #endregion

    #region Search
    //Search for entrys by Date
    public void GetEntrysByDate(Calendar date, DataGrid dgManageEntrys)
    {
      dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.Date == date.SelectedDate.Value.ToString("dd. MMMM yyyy")).ToList();
    }

    public void ShowAll(DataGrid dgManageEntrys)
    {
      dgManageEntrys.ItemsSource = lstEntry;
    }

    //Search for entrys by Tag
    public void GetEntrysByTag(CheckBox chkBxFamily, CheckBox chkBxFriends, CheckBox chkBxBirthday, DataGrid dgManageEntrys)
    {
      bool bFamily = (bool)chkBxFamily.IsChecked;
      bool bFriends = (bool)chkBxFriends.IsChecked;
      bool bBirthday = (bool)chkBxBirthday.IsChecked;

      if (bFamily == true && bFriends == false && bBirthday == false)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFamily).ToList();
      }
      else if (bFamily == false && bFriends == true && bBirthday == false)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFriends).ToList();
      }
      else if (bFamily == false && bFriends == false && bBirthday == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagBirthday).ToList();
      }
      else if (bFamily == true && bFriends == true && bBirthday == false)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFamily && lst.TagFriends).ToList();
      }
      else if (bFamily == true && bFriends == true && bBirthday == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFamily && lst.TagBirthday && lst.TagFriends).ToList();
      }
      else if (bFamily == true && bFriends == false && bBirthday == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFamily && lst.TagBirthday).ToList();
      }
      else if (bFamily == false && bFriends == true && bBirthday == true)
      {
        dgManageEntrys.ItemsSource = lstEntry.Where(lst => lst.TagFriends && lst.TagBirthday).ToList();
      }
      else
      {
        dgManageEntrys.ItemsSource = lstEntry;
      }
    }
    #endregion
    #endregion

    public byte[] ImageToByteArray()
    {
      if (selectedFileName != null)
      {
        return File.ReadAllBytes(selectedFileName);
      }
      return null;
    }

    public BitmapImage AddImage()
    {
      //Add Image
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
      if (dialog.ShowDialog() == true)
      {
        selectedFileName = dialog.FileName;
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(selectedFileName);
        bitmap.EndInit();
        return bitmap;
      }
      return null;
    }
  }
}
