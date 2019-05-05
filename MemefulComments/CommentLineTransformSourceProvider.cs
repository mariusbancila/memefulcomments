using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace MemefulComments
{
   [Export(typeof(ILineTransformSourceProvider))]
   [ContentType(ContentTypes.Cpp), 
    ContentType(ContentTypes.CSharp), 
    ContentType(ContentTypes.VisualBasic), 
    ContentType(ContentTypes.FSharp), 
    ContentType(ContentTypes.JavaScript),
    ContentType(ContentTypes.TypeScript),
    ContentType(ContentTypes.Python),
    ContentType(ContentTypes.Java)]
   [TextViewRole(PredefinedTextViewRoles.Document)]
   internal class CommentLineTransformSourceProvider : ILineTransformSourceProvider
   {
      [Import]
      public ITextDocumentFactoryService TextDocumentFactory { get; set; }

      [Import]
      internal SVsServiceProvider ServiceProvider = null;

      ILineTransformSource ILineTransformSourceProvider.Create(IWpfTextView view)
      {
         var manager = view.Properties.GetOrCreateSingletonProperty(() => new CommentsAdornment(view, TextDocumentFactory , ServiceProvider));
         return new CommentLineTransformSource(manager);
      }
   }
}
