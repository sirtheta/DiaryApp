using DiaryApp.Control;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    List<DiaryEntryDb> lstEntry = new List<DiaryEntryDb>();
    List<TagDb> lstTag = new List<TagDb>();

    public MainWindow()
    {
      InitializeComponent();
      GetCheckBoxTags();
      GetDataGridEntrys();
    }

    private void GetDataGridEntrys()
    {
      using (var db = new DiaryContext())
      {
        var query = from b in db.DiaryEntrys
                    orderby b.EntryId
                    select b;

        foreach (var item in query)
        {
          lstEntry.Add(item);
        }
      }
      dgManageEntrys.ItemsSource = lstEntry;
    }

    private void GetCheckBoxTags()
    {
      using (var db = new DiaryContext())
      {
        var query = from b in db.Tags
                    orderby b.TagID
                    select b;

        foreach (var item in query)
        {
          lstTag.Add(item);
        }
      }
      chkBxFamily.Content = lstTag[0].TagText;
      chkBxFriends.Content = lstTag[1].TagText;
      chkBxBirthday.Content = lstTag[2].TagText;
    }

    private string CreateTagText()
    {
      StringBuilder sb = new StringBuilder();
      if ((bool)chkBxFamily.IsChecked)
      {
        sb.Append(chkBxFamily.Content);
      }
      if ((bool)chkBxFriends.IsChecked)
      {
        if (sb.Length != 0)
        {
          sb.Append(", ");
        }
        sb.Append(chkBxFriends.Content);
      }
      if ((bool)chkBxBirthday.IsChecked)
      {
        if (sb.Length != 0)
        {
          sb.Append(", ");
        }
        sb.Append(chkBxBirthday.Content);
      }
      return Regex.Replace(sb.ToString(), "[^A-Za-z0-9, ]", "");
    }

    private void SaveEntry()
    {
      string date;
      if (calendar.SelectedDate != null)
      {
        date = calendar.SelectedDate.Value.ToString("dd. MMMM yyyy");
      }
      else
      {
        date = DateTime.Now.ToString("dd. MMMM yyyy");
      }

      string tag = CreateTagText();
      if (entryInputText.Text != "")
      {
        using (var db = new DiaryContext())
        {
          var newEntry = new DiaryEntryDb() { Text = entryInputText.Text, Date = date, Tag = tag };
          db.DiaryEntrys.Add(newEntry);
          db.SaveChanges();
          lstEntry.Add(newEntry);
        }
        //after adding new Entry, update the DataGrid
        dgManageEntrys.Items.Refresh();
        entryInputText.Text = "";
        Helper.ShowNotification("Success", "Your diary entry is saved successfull", NotificationType.Success);
      }
      else
      {
        Helper.ShowMessageBox("No text to Save. Please write your diarytext before saving!", MessageType.Error, MessageButtons.Ok);
      }
    }

    private void DeleteSelectedEntry()
    {


      if (dgManageEntrys.SelectedItem is DiaryEntryDb entrys)
      {
        if (Helper.ShowMessageBox("Delete selected entry?", MessageType.Confirmation, MessageButtons.YesNo))
        {
          lstEntry.Remove(entrys);
          using (var db = new DiaryContext())
          {
            db.Entry(entrys).State = EntityState.Deleted;
            db.SaveChanges();
          }
          //after deleting Entry, update the DataGrid
          dgManageEntrys.Items.Refresh();
          entryInputText.Text = "";
          Helper.ShowNotification("Success", "Diary Entry Successfull deleted", NotificationType.Success); 
        }
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }


    #region Events
    private void BtnSaveEntry_Click(object sender, RoutedEventArgs e)
    {
      SaveEntry();
    }

    private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      DeleteSelectedEntry();
    }

    private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    {
      //OpenFileDialog dialog = new OpenFileDialog();
      //dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
      //if (dialog.ShowDialog() == true)
      //{
      //  //do something with the file
      //}
    }

    //This method prevents the mous from captured inside calender
    //-->Problem without: You have to cklick twice onto the button in order to fire the click event
    protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
    {
      base.OnPreviewMouseUp(e);
      if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
      {
        Mouse.Capture(null);
      }
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void cardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    } 
    #endregion

    //For Dark Theme switch implementation:
    //  bool isDark = true;
    //  ITheme theme = _paletteHelper.GetTheme();
    //  IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
    //  theme.SetBaseTheme(baseTheme);
    //  _paletteHelper.SetTheme(theme);
    //}
    //private readonly PaletteHelper _paletteHelper = new PaletteHelper();
  }
}
