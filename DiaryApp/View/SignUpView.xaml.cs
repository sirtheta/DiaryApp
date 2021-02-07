using System.Windows;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for Registration.xaml
  /// </summary>
  public partial class SignUp : Window
  {
    readonly SignUpLogic logic;

    public SignUp()
    {
      InitializeComponent();
      logic = new SignUpLogic(txtBoxLastName, txtBoxFirstName, txtBoxUserName, txtBoxPassword, txtBoxPasswordConfirm);
    }

    private void SignUp_Click(object sender, RoutedEventArgs e)
    {
      if (logic.SignUp())
      {
        Close();
      }      
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
