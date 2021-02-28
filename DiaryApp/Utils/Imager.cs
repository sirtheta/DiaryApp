﻿using System;
using System.IO;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

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

    public byte[] ImageToByteArray(string fileUri, int width = 1024, int height = 1024)
    {
      using Image image = Image.Load(fileUri);
      int resizeMaxWidth;
      int resizeMaxHeight;
      if (image.Width > width || image.Height > height)
      {
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

      using var ms = new MemoryStream();
      image.Save(ms, new JpegEncoder());
      return ms.ToArray();
    }
  }
}
