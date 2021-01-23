using DiaryApp.Control;
using MaterialDesignThemes.Wpf;
using Notifications.Wpf.Core;
using System;
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
    //List<DiaryEntryDb> lstEntry = new List<DiaryEntryDb>();


    public MainWindow()
    {
      InitializeComponent();
      InitializeContent();
    }

    private void InitializeContent()
    {
      Logic logic = new Logic();
      var lstTag = logic.LstTag;
      chkBxFamily.Content = lstTag[0].TagText;
      chkBxFriends.Content = lstTag[1].TagText;
      chkBxBirthday.Content = lstTag[2].TagText;
      dgManageEntrys.ItemsSource = logic.LstEntry;
    }

    #region Events
    private void BtnSaveEntry_Click(object sender, RoutedEventArgs e)
    {
      Logic saveLogic = new Logic(entryInputText.Text, (bool)chkBxFamily.IsChecked, (bool)chkBxFriends.IsChecked, (bool)chkBxBirthday.IsChecked, calendar.SelectedDate.Value);
      dgManageEntrys.ItemsSource = saveLogic.SaveEntry();

      //clear text in input field and checkBoxes
      entryInputText.Text = "";
      chkBxFamily.IsChecked = false;
      chkBxFriends.IsChecked = false;
      chkBxBirthday.IsChecked = false;
    }

    private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      if (dgManageEntrys.SelectedItem is DiaryEntryDb entrys)
      {
        Logic deleteLogic = new Logic(entrys);
        dgManageEntrys.ItemsSource = deleteLogic.DeleteSelectedEntry();
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }

    private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    {  ////For Testing
       //Logic logic = new Logic(calendar.SelectedDate.Value);
       //Logic logic = new Logic((bool)chkBxFamily.IsChecked, (bool)chkBxFriends.IsChecked, (bool)chkBxBirthday.IsChecked);     
       // dgManageEntrys.ItemsSource = logic.LstEntryByDate;

      ////Add Image
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

    private void CardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }

    //private void BtnDarkSwitch_Click(object sender, RoutedEventArgs e)
    //{
    //  //For Dark Theme switch implementation:
    //  bool isDark = true;
    //  ITheme theme = _paletteHelper.GetTheme();
    //  IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
    //  theme.SetBaseTheme(baseTheme);
    //  _paletteHelper.SetTheme(theme);
    //}
    //private readonly PaletteHelper _paletteHelper = new PaletteHelper();
  }
  #endregion
}