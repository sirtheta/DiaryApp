using DiaryApp.Control;
using DiaryApp.Model;
using Notifications.Wpf.Core;
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

    public MainWindow()
    { 
      InitializeComponent();
      InitializeContent();
    }

    readonly MainWindowLogic logic = new MainWindowLogic();

    private void InitializeContent()
    {
      var lstTag = logic.LstTag;
      txtLoggedInUser.Text = logic.FullName();
      chkBxFamily.Content = lstTag[0];
      chkBxFriends.Content = lstTag[1];
      chkBxBirthday.Content = lstTag[2];
      dgManageEntrys.ItemsSource = logic.LstEntry();
    }

    #region Events
    private void BtnSaveEntry_Click(object sender, RoutedEventArgs e)
    {
      dgManageEntrys.ItemsSource = logic.SaveEntry(entryInputText.Text,
                                                   (bool)chkBxFamily.IsChecked,
                                                   (bool)chkBxFriends.IsChecked,
                                                   (bool)chkBxBirthday.IsChecked,
                                                   calendar.SelectedDate.Value);
      //Update Datagrid
      dgManageEntrys.Items.Refresh();

      //clear text in input field and checkBoxes
      entryInputText.Text = "";
      chkBxFamily.IsChecked = false;
      chkBxFriends.IsChecked = false;
      chkBxBirthday.IsChecked = false;
    }

    private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    {
      //Add Image
      imageBox.Source = logic.AddImage();
    }

    private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      if (dgManageEntrys.SelectedItem is DiaryEntryDb entrys)
      {
        dgManageEntrys.ItemsSource = logic.DeleteSelectedEntry(entrys);
        //Update Datagrid
        dgManageEntrys.Items.Refresh();
      }
      else
      {
        Helper.ShowNotification("Error", "No entry selected!", NotificationType.Error);
      }
    }

    private void BtnSearchTag_Click(object sender, RoutedEventArgs e)
    {
      dgManageEntrys.ItemsSource = logic.GetEntrysByTag((bool)chkBxFamily.IsChecked, (bool)chkBxFriends.IsChecked, (bool)chkBxBirthday.IsChecked);

    }
    private void BtnSearchDate_Click(object sender, RoutedEventArgs e)
    {
      dgManageEntrys.ItemsSource = logic.GetEntrysByDate(calendar.SelectedDate.Value);
    }

    private void BtnShowAll_Click(object sender, RoutedEventArgs e)
    {
      dgManageEntrys.ItemsSource = logic.ShowAll();
    }

    //This method prevents the mous from captured inside calender
    //-->Problem without: One have to cklick twice onto button in order to fire the click event
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