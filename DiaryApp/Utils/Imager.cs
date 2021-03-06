using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Windows.Media.Imaging;

namespace DiaryApp
{
  class Imager
  {
    public static BitmapImage ImageFromByteArray(byte[] array)
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

    public static byte[] ImageToByteArray(string fileUri, int width = 1024, int height = 1024)
    {
      using Image image = Image.Load(fileUri);
      if (image.Width > width || image.Height > height)
        ResizeImage(image, width, height);
      using var ms = new MemoryStream();
      image.Save(ms, new JpegEncoder());
      return ms.ToArray();
    }

    private static void ResizeImage(Image image, int width, int height)
    {
      int resizeMaxWidth;
      int resizeMaxHeight;
      if (image.Width > image.Height)
      {
        resizeMaxWidth = width;
        resizeMaxHeight = 0;
      }
      else
      {
        resizeMaxWidth = 0;
        resizeMaxHeight = height;
      }
      image.Mutate(x => x.Resize(resizeMaxWidth, resizeMaxHeight));
    }
  }
}
