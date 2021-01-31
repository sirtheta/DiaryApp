using DiaryApp.Control;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>

  public partial class LoginView : Window
  {
    public LoginView()
    {
      InitializeComponent();
    }

    private void Login()
    {
      LoginLogic logic = new LoginLogic(txtBoxUserName.Text, txtBoxPassword.Password);
      if (logic.Login())
      {
        Close();
      }
    }

    #region Events
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        Login();
      }
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
      Login();
    }

    private void BtnSignUpLogin_Click(object sender, RoutedEventArgs e)
    {
      new SignUp().ShowDialog();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void CardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }
    #endregion
  }
}