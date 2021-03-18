using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// 
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly MainWindowControl control;

    public MainWindow()
    {
      InitializeComponent();
      control = new MainWindowControl();
      DataContext = control;
      control.OnMainWindowLoad();
    }

    //This passes the password to the Property in Control. Binding of Passwordbox is not possible for security reason
    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
      if (this.DataContext != null)
      {
        ((dynamic)this.DataContext).SignInPassword = ((PasswordBox)sender).SecurePassword;
      }
    }

    //Clear the Passwordbox field after Sign in, not possible with binding
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter && popupSignIn.IsOpen == true)
      {
        control.SignIn();
        pwBox.Password = null;
      }
    }

    //Clear the Passwordbox field after sign in, not possible with binding
    private void BtnPopupLogin_Click(object sender, RoutedEventArgs e)
    {
      control.SignIn();
      pwBox.Password = null;
    }

    //Not possible to bind SelectedDates to Control 
    private void Calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      control.CalendarSelectedRange = calendar.SelectedDates;
      //prevents the mouse captured inside calender that one not have to click
      //twice on another control in order to use it
      Mouse.Capture(null);
    }

    //Not possible to bind SelectedItems to Control
    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      control.DatagridSelectedItems = dgManageEntrys.SelectedItems;
    }

    private void ImageBox_MouseDown(object sender, MouseButtonEventArgs e) => imgPopup.IsOpen = true;

    private void ImgPopup_MouseDown(object sender, MouseButtonEventArgs e) => imgPopup.IsOpen = false;

    //To drag the window by click and drag in headercard
    private void CardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }
    //  //For Dark Theme switch implementation:
    //  bool isDark = true;
    //  ITheme theme = _paletteHelper.GetTheme();
    //  IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
    //  theme.SetBaseTheme(baseTheme);
    //  _paletteHelper.SetTheme(theme);
    //private readonly PaletteHelper _paletteHelper = new PaletteHelper();
  }
}