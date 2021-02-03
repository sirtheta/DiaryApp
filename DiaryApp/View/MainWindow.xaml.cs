using DiaryApp.Control;
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
    readonly Control.Control control = new Control.Control();

    public MainWindow()
    {
      InitializeComponent();
      control.LoadEntrysFromDb(dgManageEntrys);
    }


    #region Events
    private void BtnSaveEntry_Click(object sender, RoutedEventArgs e)
    {
      control.SaveEntry(entryInputText, chkBxFamily, chkBxFriends, chkBxBirthday, calendar, dgManageEntrys);
    }

    private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    {
      //Add Image TODO
      //imageBox.Source = control.AddImage();
    }

    private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      control.DeleteSelectedEntry(dgManageEntrys);
    }

    private void BtnSearchTag_Click(object sender, RoutedEventArgs e)
    {
      control.GetEntrysByTag(chkBxFamily, chkBxFriends, chkBxBirthday, dgManageEntrys);
    }
    private void BtnSearchDate_Click(object sender, RoutedEventArgs e)
    {
      control.GetEntrysByDate(calendar, dgManageEntrys);
    }

    private void BtnShowAll_Click(object sender, RoutedEventArgs e)
    {
      control.ShowAll(dgManageEntrys);
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