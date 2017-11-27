using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace MemefulComments
{
   public class CommentImage : Image
   {
      /// <summary>
      /// Location from where the image was loaded.
      /// </summary>
      public string Url { get; private set; }

      /// <summary>
      /// Original location from which the image was downloaded.
      /// </summary>
      public string OriginalUrl { get; private set; }

      /// <summary>
      /// Scale image if value is greater than 0, otherwise use source dimensions
      /// </summary>
      public double Scale
      {
         get { return _scale; }
         set
         {
            _scale = value;
            if (this.Source != null)
            {
               if (value > 0)
               {
                  this.Width = this.Source.Width * value;
                  this.Height = this.Source.Height * value;
               }
               else
               {
                  this.Width = this.Source.Width;
                  this.Height = this.Source.Height;
               }
            }
         }
      }

      public bool TrySet(string imageUrl, string originalUrl, double scale, string filepath, out Exception exception)
      {
         exception = null;
         try
         {
            Source = LoadImage(imageUrl, filepath);
            Url = imageUrl;
            OriginalUrl = originalUrl;
            Scale = scale;

            return true;
         }
         catch (Exception ex)
         {
            exception = ex;
            return false;
         }
      }

      private bool IsAbsoluteUri(string uri)
      {
         return Uri.TryCreate(uri, UriKind.Absolute, out Uri result);
      }

      private BitmapImage LoadImage(string uri, string filepath)
      {
         BitmapImage image = new BitmapImage()
         {
            CacheOption = BitmapCacheOption.OnLoad,
         };

         if(!IsAbsoluteUri(uri))
         {
            uri = Path.Combine(Path.GetDirectoryName(filepath), uri);
         }

         image.BeginInit();
         image.UriSource = new Uri(uri, UriKind.Absolute);
         image.EndInit();         

         if(!image.IsDownloading)
         {
            if(image.CanFreeze)
               image.Freeze();
         }
         else
         {
            image.DownloadCompleted += (s, e) =>
            {
               if ((s as BitmapImage).CanFreeze)
                  (s as BitmapImage).Freeze();
            };
         }

         if(Path.GetExtension(uri).ToLower() == ".gif")
            ImageBehavior.SetAnimatedSource(this, image);

         return image;
      }      

      public override string ToString()
      {
         return Url;
      }

      private double _scale;
   }
}