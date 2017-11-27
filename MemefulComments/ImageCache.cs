using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemefulComments
{
   class ImageCache
   {
      ThreadSafeDictionary<string, string> _imageMap = new ThreadSafeDictionary<string, string>();
      private ImageCache() { }
      ~ImageCache()
      {
         foreach(var path in _imageMap.Values)
         {
            try
            {
               File.Delete(path);
            }
            catch { }
         }
      }

      private static ImageCache _instance;
      private static object _lock = new object();

      public static ImageCache Instance
      {
         get
         {
            if(_instance == null)
            {
               lock(_lock)
               {
                  if(_instance == null)
                  {
                     _instance = new ImageCache();
                  }
               }
            }

            return _instance;
         }
      }
         
      public void Add(string uri, string local)
      {
         _imageMap.Add(uri, local);
      }

      public bool TryGetValue(string uri, out string local)
      {
         return _imageMap.TryGetValue(uri, out local);
      }
   }
}
