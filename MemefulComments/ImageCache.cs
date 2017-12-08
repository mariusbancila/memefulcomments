using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemefulComments
{
   class ImageCache
   {
      ConcurrentDictionary<string, string> _imageMap = new ConcurrentDictionary<string, string>();
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

      private static Lazy<ImageCache> _instance = new Lazy<ImageCache>(true);

      public static ImageCache Instance
      {
         get
         {
            return _instance.Value;
         }
      }

      public void Add(string uri, string local)
      {
         _imageMap.GetOrAdd(uri, local);
      }

      public bool TryGetValue(string uri, out string local)
      {
         return _imageMap.TryGetValue(uri, out local);
      }
   }
}
