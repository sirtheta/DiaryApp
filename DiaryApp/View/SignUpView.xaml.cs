using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for Registration.xaml
  /// </summary>
  public partial class SignUp : Window
  {
    readonly SignUpControl signUpControl;

    public SignUp()
    {
      InitializeComponent();
      signUpControl = new SignUpControl();
      DataContext = signUpControl;
    }

    //This passes the password to the Property in Control. Binding of Passwordbox is not possible for security reason
    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
      if (this.DataContext != null)
      {
        ((dynamic)this.DataContext).SignInPassword = ((PasswordBox)sender).SecurePassword;
      }
    }
    private void PasswordConfirmChanged(object sender, RoutedEventArgs e)
    {
      if (this.DataContext != null)
      {
        ((dynamic)this.DataContext).SignInPassword = ((PasswordBox)sender).SecurePassword;
      }
    }

    private void SignUp_Click(object sender, RoutedEventArgs e)
    {
      if (signUpControl.SignUp())
      {
        Close();
      }
      //Clear the Passwordbox fields
      pwBox.Password = null;
      pwBoxConfirm.Password = null;
    }
    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void CardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }
  }
}
