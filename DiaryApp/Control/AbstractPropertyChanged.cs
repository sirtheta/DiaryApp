using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiaryApp
{
  //Baseclass for INotifyPropertyChanged
  abstract class AbstractPropertyChanged : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
