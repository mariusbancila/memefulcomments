using Microsoft.VisualStudio.Shell;
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
    ContentType(ContentTypes.TypeScript),
    ContentType(ContentTypes.Python),
    ContentType(ContentTypes.Java)]
   [TagType(typeof(ErrorTag))]
   internal class ErrorTaggerProvider : IViewTaggerProvider
   {
      [Import]
      public ITextDocumentFactoryService TextDocumentFactory { get; set; }

      [Import]
      internal SVsServiceProvider ServiceProvider = null;

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

         var imageAdornmentManager = textView.Properties.GetOrCreateSingletonProperty(() => new CommentsAdornment((IWpfTextView)textView, TextDocumentFactory, ServiceProvider));
         return imageAdornmentManager as ITagger<T>;
      }
   }
}
