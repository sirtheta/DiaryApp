﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiaryApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly Control control;

    public MainWindow()
    {
      InitializeComponent();
      control = new Control();
      DataContext = control;
      popupLogin.IsOpen = true;
    }

    //This passes the password to the Property in Control. Binding of Passwordbox is not possible
    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
      if (this.DataContext != null) ((dynamic)this.DataContext).SignInPassword = ((PasswordBox)sender).SecurePassword;
    }

    private void SignIn()
    {
      if (control.VerifyCredentials())
      {
        popupLogin.IsOpen = false;
        mainStackPanel.IsEnabled = true;
        btnLogin.Visibility = Visibility.Hidden;
        btnSignOut.Visibility = Visibility.Visible;
      }
      //Clear the Passwordbox field
      pwBox.Password = null;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter && popupLogin.IsOpen == true)
      {
        SignIn();
      }
    }

    private void BtnPopupLogin_Click(object sender, RoutedEventArgs e)
    {
      SignIn();
    }

    private void BtnSignOut_Click(object sender, RoutedEventArgs e)
    {
      control.SignOut();
      mainStackPanel.IsEnabled = false;
      btnLogin.Visibility = Visibility.Visible;
      btnSignOut.Visibility = Visibility.Hidden;
    }

    private void BtnSignUpLogin_Click(object sender, RoutedEventArgs e)
    {
      new SignUp().ShowDialog();
    }

    private void CloseLoginPopup(object sender, RoutedEventArgs e)
    {
      popupLogin.IsOpen = false;
    }

    private void ImageBox_MouseDown(object sender, MouseButtonEventArgs e)
    {
      imgPopup.IsOpen = true;
    }

    private void ImgPopup_MouseDown(object sender, MouseButtonEventArgs e)
    {
      imgPopup.IsOpen = false;
    }

    private void BtnSaveEntry_Click(object sender, RoutedEventArgs e)
    {
      control.SaveEntry();
    }

    private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    {
      control.AddImage();
    }

    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
      control.ClearControls();
    }

    private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      control.DeleteSelectedEntry();
    }

    private void BtnSearchTag_Click(object sender, RoutedEventArgs e)
    {
      control.GetEntrysByTag();
    }

    private void BtnSearchDate_Click(object sender, RoutedEventArgs e)
    {
      control.GetEntrysByDate();
    }

    private void BtnShowAll_Click(object sender, RoutedEventArgs e)
    {
      control.ShowAll();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      control.ShowSelectedItem();
      //Not possible to bind SelectedItems to Control
      control.SelectedItems = dgManageEntrys.SelectedItems;
    }



    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
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
}