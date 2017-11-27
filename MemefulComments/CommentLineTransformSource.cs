using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace MemefulComments
{
   internal class CommentLineTransformSource : ILineTransformSource
   {
      private CommentsAdornment _adornment;

      public CommentLineTransformSource(CommentsAdornment adornment)
      {
         _adornment = adornment;
      }

      LineTransform ILineTransformSource.GetLineTransform(
         ITextViewLine line, 
         double yPosition, 
         ViewRelativePosition placement)
      {
         var lineNumber = line.Snapshot.GetLineFromPosition(line.Start.Position).LineNumber;

         // Look up Image for current line and increase line height as necessary
         if (CommentsAdornment.Enabled && 
            _adornment.Images.ContainsKey(lineNumber) &&
            _adornment.Images[lineNumber].Source != null)
         {
            var defaultHeight = line.DefaultLineTransform.BottomSpace;
            return new LineTransform(0, _adornment.Images[lineNumber].Height + defaultHeight, 1.0);
         }

         return new LineTransform(0, 0, 1.0);
      }
   }
}
