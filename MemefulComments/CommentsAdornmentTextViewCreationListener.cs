using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text;

namespace MemefulComments
{
   /// <summary>
   /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
   /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
   /// </summary>
   [Export(typeof(IWpfTextViewCreationListener))]
   [ContentType(ContentTypes.Cpp),
    ContentType(ContentTypes.CSharp),
    ContentType(ContentTypes.VisualBasic),
    ContentType(ContentTypes.FSharp),
    ContentType(ContentTypes.JavaScript),
    ContentType(ContentTypes.TypeScript)]
   [TextViewRole(PredefinedTextViewRoles.Document)]
   internal sealed class CommentsAdornmentTextViewCreationListener : IWpfTextViewCreationListener
   {
      // Disable "Field is never assigned to..." and "Field is never used" compiler's warnings. Justification: the field is used by MEF.
#pragma warning disable 649, 169

      /// <summary>
      /// Defines the adornment layer for the adornment. This layer is ordered
      /// after the selection layer in the Z-order
      /// </summary>
      [Export(typeof(AdornmentLayerDefinition))]
      [Name("CommentImageAdornmentLayer")]
      [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
      private AdornmentLayerDefinition editorAdornmentLayer;

#pragma warning restore 649, 169

      [Import]
      public ITextDocumentFactoryService TextDocumentFactory { get; set; }

      #region IWpfTextViewCreationListener

      /// <summary>
      /// Called when a text view having matching roles is created over a text data model having a matching content type.
      /// Instantiates a CommentsAdornment manager when the textView is created.
      /// </summary>
      /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
      public void TextViewCreated(IWpfTextView textView)
      {
         textView.Properties.GetOrCreateSingletonProperty(() => new CommentsAdornment(textView, TextDocumentFactory));
      }

      #endregion
   }
}
