using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer.Classifier
{
    internal static class DimFormatClassifierTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Alpzy/DimFormatDefinition")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}
