using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace MemefulComments
{
   [Export(typeof(IViewTaggerProvider))]
   [ContentType(ContentTypes.Cpp),
    ContentType(ContentTypes.CSharp),
    ContentType(ContentTypes.VisualBasic),
    ContentType(ContentTypes.FSharp),
    ContentType(ContentTypes.JavaScript),
    ContentType(ContentTypes.TypeScript)]
   [TagType(typeof(ErrorTag))]
   internal class ErrorTaggerProvider : IViewTaggerProvider
   {
      [Import]
      public ITextDocumentFactoryService TextDocumentFactory { get; set; }

      public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) 
         where T : ITag
      {
         if (textView == null)
         {
            return null;
         }

         if (textView.TextBuffer != buffer)
         {
            return null;
         }

         Trace.Assert(textView is IWpfTextView);

         var imageAdornmentManager = textView.Properties.GetOrCreateSingletonProperty(() => new CommentsAdornment((IWpfTextView)textView, TextDocumentFactory));
         return imageAdornmentManager as ITagger<T>;
      }
   }
}
