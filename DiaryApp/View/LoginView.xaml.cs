using DiaryApp.Control;
using System.Windows;
using System.Windows.Input;

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
      //if password/username is invalid
      if (txtBoxUserName.Text != "1" && txtBoxPassword.Password != "1")
      {
        Helper.ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
      }
      else
      {
        Window main = new MainWindow();
        main.Show();
        this.Close();
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