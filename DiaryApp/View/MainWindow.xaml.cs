
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
    Control control;

    public MainWindow(Control t_control)
    {
      InitializeComponent();
      control = t_control;
      control.SetReferences(entryInputText, chkBxFamily, chkBxFriends, chkBxBirthday, calendar, dgManageEntrys, imageBox);
      control.LoadEntrysFromDb();
    }


    #region Events
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
      control.ShowAll(dgManageEntrys);
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      control.ShowSelectedItem();
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

    private void ImageBox_MouseDown(object sender, MouseButtonEventArgs e)
    {
      imgPopup.IsOpen = true;
      imageInPopup.Source = imageBox.Source;
    }

    private void ImgPopup_MouseDown(object sender, MouseButtonEventArgs e)
    {
      imgPopup.IsOpen = false;
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