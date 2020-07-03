using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer.Dim_Classifier
{
    [Export(typeof(IClassifierProvider))]
    [ContentType("code")] // This classifier applies to all text files.
    internal class DimClassifierProvider : IClassifierProvider
    {
        [Import]
        private IClassificationTypeRegistryService classificationRegistry;

        #region IClassifierProvider
        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(creator: () => new DimClassifier(this.classificationRegistry));
        }

        #endregion
    }
}
