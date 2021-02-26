using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  class Imager
  {
    public BitmapImage BitmapForImageSource(string fileUri)
    {
      BitmapImage bitmap = new BitmapImage();
      bitmap.BeginInit();
      bitmap.UriSource = new Uri(fileUri);
      bitmap.EndInit();
      return bitmap;
    }

    public BitmapImage ImageFromByteArray(byte[] array)
    {
      if (array != null)
      {
        var image = new BitmapImage();
        using var ms = new MemoryStream(array);
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = ms;
        image.EndInit();
        return image;
      }
      return null;
    }
    public byte[] ImageToByteArray(string fileUri)
    {
      using Image image = Image.Load(fileUri);
      int width;
      int height;
      if (image.Width > image.Height)
      {
        width = 1024;
        height = 0;
      }
      else
      {
        width = 0;
        height = 1024;
      }
      image.Mutate(x => x.Resize(width, height));

      using var ms = new MemoryStream();
      image.Save(ms, new JpegEncoder());
      return ms.ToArray();
    }
  }
}
