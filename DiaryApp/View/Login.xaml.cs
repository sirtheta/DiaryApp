using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>

  public partial class Login : Window
  {
    public Login()
    {
      InitializeComponent();
    }
    public static void ShowMessageBox(string messageStr, MessageType type, MessageButtons buttons) => new CustomMessageBox(messageStr, type, buttons).ShowDialog();

    public bool LoginSuccess { get; set; }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
      //if password/username is invalid
      if (txtBoxUserName.Text != "1" && txtBoxPassword.Password != "1")
      {
        ShowMessageBox("Login incorrect, try again!", MessageType.Error, MessageButtons.Ok);
      }
      else
      {
        Window main = new MainWindow();
        main.Show();
      }
    }

    private void BtnSignUpLogin_Click(object sender, RoutedEventArgs e)
    {
      new SignUp().ShowDialog();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void cardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }
  }
}