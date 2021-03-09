using System;
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
      //To close the window from signUpControl
      if (signUpControl.CloseAction == null)
        signUpControl.CloseAction = new Action(this.Close);
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
        ((dynamic)this.DataContext).SignInPasswordConfirm = ((PasswordBox)sender).SecurePassword;
      }
    }

    //Clear the Passwordbox fields after Login. No Binding possible
    private void SignUp_Click(object sender, RoutedEventArgs e)
    {
      signUpControl.SignUp();
      pwBox.Password = null;
      pwBoxConfirm.Password = null;
    }

    //To drag the window by click and drag in header
    private void CardHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }
  }
}
