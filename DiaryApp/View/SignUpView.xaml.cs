using System.Windows;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for Registration.xaml
  /// </summary>
  public partial class SignUp : Window
  {
    public SignUp()
    {
      InitializeComponent();
    }

    private void SignUp_Click(object sender, RoutedEventArgs e)
    {
      Close();
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
