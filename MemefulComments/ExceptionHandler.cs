using System;

namespace MemefulComments
{
   internal static class ExceptionHandler
   {
      public static void Notify(Exception ex, bool showMessage)
      {
         string message = $"{DateTime.Now}: {ex.ToString()}";
         Console.WriteLine(message);
         if (showMessage)
         {
            UIMessage.Show(message);
         }
      }
   }
}
